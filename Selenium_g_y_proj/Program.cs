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
       
        static void Main(string[] args)
        {

            try
            {
                int offset = 0;
                int limit = 100;
                DBConection db = new DBConection();
                int count = db.count();
                while (offset < count) {
                    foreach (DBKeyword k in db.list(offset, limit))
                    {
                        Google google = new Google("https://google.com.ua");
                        //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                        google.search(k.keyword, k.keyword_id);
                        google.exit();

                        Yandex yandex = new Yandex("https://yandex.ru");
                        //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                        yandex.search(k.keyword, k.keyword_id);
                        yandex.exit();
                    }
                    
                     offset += 100;                   
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
