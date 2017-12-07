using System;
using System.Configuration;
using System.Diagnostics;

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

            //StartProxyServer();

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
                                google = new Google("http://google.ru");
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
                        //if (!MyIni.KeyExists("offset_yandex"))
                        //{
                        //    MyIni.Write("offset_yandex", "0");
                        //}
                        //else
                        //    offset = Int32.Parse(MyIni.Read("offset_yandex"));

                        //if (!MyIni.KeyExists("limit_yandex"))
                        //{
                        //    MyIni.Write("limit_yandex", "" + STEP);
                        //}
                        //else
                        //    limit = Int32.Parse(MyIni.Read("limit_yandex"));

                        //yandex = new Yandex("https://yandex.ru", MODE);
                        //yandex.open_settings();
                        //while (offset < count)
                        //{
                        //    foreach (DBKeyword k in db.list(offset, limit))
                        //    {
                        //        Console.WriteLine("Выбираем словосочетание из БД="
                        //            + k.keyword + "[" + k.keyword_id + "] offset="
                        //            + offset + " limit=" + limit);
                        //        Console.WriteLine("Обращаемся к яндексу");
                        //        //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                        //        yandex.search(k.keyword, k.keyword_id);
                        //    }

                        //    offset += STEP;
                        //    MyIni.Write("offset_yandex", "" + offset);
                        //}
                        //yandex.exit();


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

                        google = new Google("http://google.ru", MODE);
                        while (offset < count)
                        {
                            foreach (DBKeyword k in db.list(offset, limit))
                            {
                                Console.WriteLine("Выбираем словосочетание из БД="
                                    + k.keyword + "[" + k.keyword_id + "] offset="
                                    + offset + " limit=" + limit);

                                Console.WriteLine("Обращаемся к гуглу");
                                //открываем настройки и ставим регион 
                                google.open_settings();
                                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                                google.search(k.keyword, k.keyword_id);

                            }
                            offset += STEP;
                            MyIni.Write("offset_google", "" + offset);
                        }
                        google.exit();
                        break;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);               
            }

            //закрываем окно
            Environment.Exit(0);

        }

        public static void StartProxyServer()
        {
            StopProxyServer();

            Process proxySrv = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ConfigurationManager.AppSettings["TorProxySrvLocation"],
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = " -f \"C:\\Users\\exxxa\\Desktop\\Tor Browser\\Browser\\TorBrowser\\Data\\Tor\\torrc\""
                }
            };

            proxySrv.Start();
       
        } 

        // Останов работы прокси-сервера
        public static void StopProxyServer()
        {
            try
            {
                // Находим запущенный процесс прокси-сервера
                Process[] processes = Process.GetProcessesByName("tor");

                // Если он запущен, перезапускаем его
                if (processes.Length > 0) processes[0].Kill();
            }catch(Exception e) {}            
        } 

    }
}
