using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    class Utils:DBConection
    {
        public Utils() : base()
        {

        }

        private bool VerifyInternetConnection(String url)
        {

            IPStatus status = IPStatus.TimedOut;
            try
            {
                Regex rgx = new Regex("(https?://)");
                Console.WriteLine(rgx.Replace(url, ""));

                Ping ping = new Ping();
                PingReply reply = ping.Send(rgx.Replace(url, ""));
                status = reply.Status;
                Console.WriteLine("Internet STATUS is " + status);
            }
            catch 
            {
                return false;
            }

            return (status == IPStatus.Success) ;
        }

        public void VerifyPageIsLoaded(ChromeDriver driver)
        {
            String CurrentURL = driver.Url;

            //Get and store domain name in variable
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            string DomainUsingJS = (string)js.ExecuteScript("return document.domain");

            while (!VerifyInternetConnection(DomainUsingJS))
            {
                Console.WriteLine("Упс, интернета нет, засыпаем на 1 минуту");
                System.Threading.Thread.Sleep(60000);
            }
            while (!driver.ExecuteScript("return document.readyState").Equals("complete"))
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
