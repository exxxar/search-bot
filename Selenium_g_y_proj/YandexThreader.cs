using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Selenium_g_y_proj.DBConection;

namespace Selenium_g_y_proj
{
    class YandexThreader
    {
        //private static Mutex mut = new Mutex();
        public const int MODE = 0;
        public const int STEP = 5;

        private Thread thread;
        private int threadId;


        private IniFiles MyIni;


        public YandexThreader(int id,int limit,int offset) //Конструктор получает имя функции и номер до кторого ведется счет
        {

            this.threadId = id;

            MyIni = new IniFiles("Settings.ini");

            List<int> settings = new List<int>();

            settings.Insert(0, limit);//limit
            settings.Insert(1, offset);//offset

            thread = new Thread(this.func);
            thread.Name = "Thread_Yandex_"+id;
            thread.Start(settings);//передача параметра в поток
        }

        void func(object list_params)//Функция потока, передаем параметр
        {

            int limit = ((List<int>)list_params)[0];
            int offset = ((List<int>)list_params)[1];

            try
            {
                //mut.WaitOne();
                DBConection db = new DBConection();
                int count = db.count();
                

                Yandex yandex = new Yandex("https://yandex.ru", MODE);
                if (!yandex.isValidRegion("Москва"))
                    yandex.open_settings();

                yandex.change_search_pos_count();

               // mut.ReleaseMutex();

                while (offset < count)
                {
                    foreach (DBKeyword k in db.list(offset, limit))
                    {
                        Console.WriteLine("Выбираем словосочетание из БД="
                            + k.keyword + "[" + k.keyword_id + "] offset="
                            + offset + " limit=" + limit);
                        Console.WriteLine("Обращаемся к яндексу");
                        //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                       // mut.WaitOne();
                        yandex.search(k.keyword, k.keyword_id);
                       // mut.ReleaseMutex();
                    }

                    offset += STEP;
                    MyIni.Write("offset_yandex_"+this.threadId, "" + offset);
                }
                yandex.exit();
              
            }
            catch
            {

            }
        }

    }
}
