using OpenQA.Selenium.Chrome;
using System;

using System.Configuration;
using System.Diagnostics;



namespace Selenium_g_y_proj
{
    class Program
    {
        
        public const int STEP = 5;
        public const int MODE = 1;

        static void Main(string[] args)
        {
            YandexThreader yThread2 = new YandexThreader(1, 5, 0);

            return;
            int offset = 0;
            int limit = STEP;
            int threadsCount = 3;

            DBConection db = new DBConection();
            int count = db.count();
            int offsetStepByThread = (int)(count / threadsCount);

            var MyIni = new IniFiles("Settings.ini");

            threadsCount = !MyIni.KeyExists("thread_count") ?
                  Int32.Parse(MyIni.Write("thread_count", "" + threadsCount)):
                  Int32.Parse(MyIni.Read("thread_count"));

            Console.WriteLine("Потоков: {0} , смещение в бд для 1 потока {1}", threadsCount, offsetStepByThread);
            Console.ReadKey();
            for (int i = 0; i < threadsCount; i++)
            {
                offset = !MyIni.KeyExists("offset_yandex_"+i) ?
                 Int32.Parse(MyIni.Write("offset_yandex_"+i, "" + (offsetStepByThread * i))) :
                  Int32.Parse(MyIni.Read("offset_yandex_"+i));

                limit = !MyIni.KeyExists("limit_yandex_"+i) ? 
                   Int32.Parse(MyIni.Write("limit_yandex_"+i, "" + STEP)) :
                   Int32.Parse(MyIni.Read("limit_yandex_"+i));

                Console.WriteLine("id={0}  limit=>{1} offset=>{2}",i,limit,offset);
 
                YandexThreader yThread = new YandexThreader(i, limit, offset);

                //offset = !MyIni.KeyExists("offset_google_" + i) ?
                // Int32.Parse(MyIni.Write("offset_google_" + i, "" + (threadsCount * i))) :
                //  Int32.Parse(MyIni.Read("offset_google_" + i));

                //limit = !MyIni.KeyExists("limit_google_" + i) ?
                //   Int32.Parse(MyIni.Write("limit_google_" + i, "" + STEP)) :
                //   Int32.Parse(MyIni.Read("limit_google_" + i));


                //GoogleThreader gThread = new GoogleThreader(i, limit, offset);
                //Console.WriteLine("test limit=>{0} offset=>{1}", limit, offset);

            }

            Console.ReadKey();
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
