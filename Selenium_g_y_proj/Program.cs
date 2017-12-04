using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Selenium_g_y_proj.DBConection;

namespace Selenium_g_y_proj
{
    class Program
    {

        public const int STEP = 5;
        public const int MODE = 1;
        static void Main(string[] args)
        {
            
            int offset = 0;
            int limit = STEP;

            var MyIni = new IniFiles("Settings.ini");

           

            try
            {

                Google google = null;
                Yandex yandex = null;
                DBConection db = new DBConection();
                int count = db.count();
                switch (MODE)
                {
                    default:
                    case 0:
                        if (!MyIni.KeyExists("offset"))
                        {
                            MyIni.Write("offset", "0");
                        }
                        else
                            offset = Int32.Parse(MyIni.Read("offset"));

                        if (!MyIni.KeyExists("limit"))
                        {
                            MyIni.Write("limit", "" + STEP);
                        }
                        else
                            limit = Int32.Parse(MyIni.Read("limit"));

                        while (offset < count)
                        {
                            foreach (DBKeyword k in db.list(offset, limit))
                            {
                                Console.WriteLine("Выбираем словосочетание из БД="
                                    + k.keyword + "[" + k.keyword_id + "] offset="
                                    + offset + " limit=" + limit);

                                Console.WriteLine("Обращаемся к гуглу");
                                google = new Google("https://google.com.ua");
                                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                                google.search(k.keyword, k.keyword_id);
                                google.exit();

                                Console.WriteLine("Обращаемся к яндексу");
                                yandex = new Yandex("https://yandex.ru");
                                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                                yandex.search(k.keyword, k.keyword_id);
                                yandex.exit();
                            }

                            offset += STEP;
                            MyIni.Write("offset", "" + offset);
                        }
                        break;

                    case 1:
                        if (!MyIni.KeyExists("offset_google"))
                        {
                            MyIni.Write("offset_google", "0");
                        }
                        else
                            offset = Int32.Parse(MyIni.Read("offset_google"));

                        if (!MyIni.KeyExists("limit_google"))
                        {
                            MyIni.Write("limit_google", "" + STEP);
                        }
                        else
                            limit = Int32.Parse(MyIni.Read("limit_google"));

                        google = new Google("https://google.com.ua",MODE);
                        while (offset < count)
                        {
                            foreach (DBKeyword k in db.list(offset, limit))
                            {
                                Console.WriteLine("Выбираем словосочетание из БД="
                                    + k.keyword + "[" + k.keyword_id + "] offset="
                                    + offset + " limit=" + limit);

                                Console.WriteLine("Обращаемся к гуглу");                            
                                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                                google.search(k.keyword, k.keyword_id);
                                                             
                            }                           
                            offset += STEP;
                           MyIni.Write("offset_google", "" + offset);
                        }
                        google.exit();

                        if (!MyIni.KeyExists("offset_yandex"))
                        {
                            MyIni.Write("offset_yandex", "0");
                        }
                        else
                            offset = Int32.Parse(MyIni.Read("offset_yandex"));

                        if (!MyIni.KeyExists("limit_yandex"))
                        {
                            MyIni.Write("limit_yandex", "" + STEP);
                        }
                        else
                            limit = Int32.Parse(MyIni.Read("limit_yandex"));

                        yandex = new Yandex("https://yandex.ru", MODE);
                        while (offset < count)
                        {
                            foreach (DBKeyword k in db.list(offset, limit))
                            {
                                Console.WriteLine("Выбираем словосочетание из БД="
                                    + k.keyword + "[" + k.keyword_id + "] offset="
                                    + offset + " limit=" + limit);

                                Console.WriteLine("Обращаемся к яндексу");                               
                                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                                yandex.search(k.keyword, k.keyword_id);
                                
                            }
                           
                            offset += STEP;
                            MyIni.Write("offset_yandex", "" + offset);
                        }
                        yandex.exit();
                        break;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);               
            }

            //закрываем окно
            Environment.Exit(0);

        }
    }
}
