using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    class Program
    {
       
        static void Main(string[] args)
        {
            try
            {
                Google chrome = new Google("https://google.com.ua");
                //тут выбираем из базы слова и в цикле по очереди вызываем метод поиска
                chrome.search("грузчики",1);

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            
        }
    }
}
