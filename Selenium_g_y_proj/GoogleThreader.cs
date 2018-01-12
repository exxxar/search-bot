using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Selenium_g_y_proj.DBConection;

namespace Selenium_g_y_proj
{
    class GoogleThreader
    {

        public const int MODE = 1;
        public const int STEP = 5;

        private Thread thread;

        private int threadId;

        private IniFiles MyIni;


        public GoogleThreader(int id, int limit, int offset) //Конструктор получает имя функции и номер до кторого ведется счет
        {

            this.threadId = id;

            MyIni = new IniFiles("Settings.ini");

            List<int> settings = new List<int>();

            settings.Insert(0, limit);//limit
            settings.Insert(1, offset);//offset

            thread = new Thread(this.func);
            thread.Name = "Thread_Google_" + id;
            thread.Start(settings);//передача параметра в поток
            
        }

        void func(object list_params)//Функция потока, передаем параметр
        {


            int limit = ((List<int>)list_params)[0];
            int offset = ((List<int>)list_params)[1];

            try
            {
                DBConection db = new DBConection();
                int count = db.count();

                Google google = new Google("http://google.ru", MODE);
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
                    MyIni.Write("offset_google_"+this.threadId, "" + offset);
                } 
                google.exit();
            }
            catch {}
        }
    }
}
