using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    class Google:DBConection,WebSite
    {
        public String url = "http://google.com.ua";
        public ChromeDriver driver;

        public Google(String url)
        {
            this.url = url;
            driver = new ChromeDriver();//открываем сам браузер
            
            driver.Manage().Window.Maximize();//открываем браузер на полный экран
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30); //время ожидания компонента страницы после загрузки страницы
        }

        public void search(String keyword,int keyword_id)
        {
            driver.Navigate().GoToUrl(this.url);//переходим по адресу поисковика

            IWebElement text = driver.FindElement(By.Id("lst-ib"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(text).Click().Perform();  
                
            text.SendKeys(keyword);//вводим искомое словосочетание
            System.Threading.Thread.Sleep(5000);//засыпаем, чтоб на нас не подумали что мы бот
            text.SendKeys(Keys.Enter);//жмем Enter для  отправки поискового запроса
            System.Threading.Thread.Sleep(5000);//засыпаем опять же, чтоб нас не раскрыли:D мы коварны     

            //5раз обновляем страницу (пока так) чтоб выбрать разные варианты рекламы, в бд тоже нужно сделать структуру, которая бы сохраняла позицию выдачи рекламы в бд
            for (int j = 0; j < 5; j++)
            {
                int searchPos = 0;

                driver.Navigate().Refresh();//обновление страницы, чтоб выбрать больше вариантов рекламы
                System.Threading.Thread.Sleep(2000);

                text = driver.FindElement(By.Id("lst-ib"));
                actions = new Actions(driver);
                actions.MoveToElement(text).Click().Perform();

                text.Clear();
                text.SendKeys(keyword);//вводим искомое словосочетание на всякий случай повторно
                System.Threading.Thread.Sleep(1000);//засыпаем, чтоб на нас не подумали что мы бот
                text.SendKeys(Keys.Enter);//жмем Enter для  отправки поискового запроса

                if (isSelectorExist(By.CssSelector(".ads-ad"))) {//если реклама найдена
                    //выбираем все блоки, которые относятся к рекламе 
                    foreach (IWebElement i in driver.FindElements(By.CssSelector(".ads-ad")))
                    {
                        ++searchPos;
                        String url = i.FindElement(By.CssSelector("._Jwu")).Text;
                        String description = i.FindElement(By.CssSelector("._WGk")).Text;
                        //разбираем рекламное сообщение
                        Console.WriteLine(url + " " + description);

                        //формируем новую запись в бд
                        Keyword kw = new Keyword();
                        kw.url = url;
                        kw.keyword_id = keyword_id;
                        kw.description = description;
                        kw.position[j] = (byte)searchPos;
                        kw.browser = Keyword.Browser.GOOGLE;

                        if (isExist(kw))
                            Update(kw);
                        else
                            Insert(kw);

                    }

                }
                else
                {
                    driver.Navigate().Refresh();//обновление страницы, чтоб выбрать больше вариантов рекламы
                }

            }
            exit();
        }

        public void exit()
        {
            //закрываем драйвер и закрываем браузер
            driver.Close();
            driver.Quit();
            //Environment.Exit(0);
        }

        public bool isSelectorExist(By selector)
        {
            return driver.FindElements(By.CssSelector(".ads-ad")).Count == 0 ? false : true;
        }
    }
}
