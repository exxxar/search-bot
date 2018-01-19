using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;

namespace Selenium_g_y_proj
{
    class Google : Utils, WebSite
    {
        public const int MAX_REFRESHES = 1;//количество обновлений страницы для выборки
        public String url = "http://google.ru";
        public ChromeDriver driver;
        public int mode = 0;
        public int regionId = 1;

        public Google(String url, int mode = 0) : base()
        {
            this.mode = mode;
            this.url = url;

            var options = new ChromeOptions();
            //options.AddArgument("no-sandbox");
            options.AddArguments("--disable-extensions");
            // options.AddArgument("no-sandbox");
            //options.AddArgument("--incognito");
           // options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");  //--disable-media-session-api
                                                   //options.AddArgument("--remote-debugging-port=9222");

            driver = new ChromeDriver(options);//открываем сам браузер

            driver.LocationContext.PhysicalLocation = new OpenQA.Selenium.Html5.Location(55.751244, 37.618423, 152);

            driver.Manage().Window.Maximize();//открываем браузер на полный экран
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //время ожидания компонента страницы после загрузки страницы
            if (this.mode == 1)
            {
                driver.Navigate().GoToUrl(this.url);//переходим по адресу поисковика
            }
        }

        public void open_settings()
        {
            IWebElement element = null;
            Actions actions = null;

            element = driver.FindElement(By.CssSelector("._Vbu"));

            if (element.Text.ToUpper().Equals("РОССИЯ"))
                return;

            driver.Navigate().GoToUrl(this.url + "/preferences?hl=ru");

            System.Threading.Thread.Sleep(2000);

            element = driver.FindElement(By.CssSelector(".goog-slider-thumb"));
            actions = new Actions(driver);
            actions.DragAndDropToOffset(element, 100, 0).Perform();

            System.Threading.Thread.Sleep(5000);

            element = driver.FindElement(By.Id("regionanchormore"));
            actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();
            System.Threading.Thread.Sleep(2000);//засыпаем, чтоб на нас не подумали что мы бот


            element = driver.FindElement(By.Id("regionoRU"));
            actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();
            System.Threading.Thread.Sleep(2000);//засыпаем, чтоб на нас не подумали что мы бот

            element = driver.FindElement(By.CssSelector(".jfk-button-action"));
            actions = new Actions(driver);
            actions.MoveToElement(element).Click().Perform();
            System.Threading.Thread.Sleep(2000);
            driver.SwitchTo().Alert().Accept();
            //driver.Navigate().GoToUrl(this.url);//переходим по адресу поисковика
            System.Threading.Thread.Sleep(4000);
        }
        public void search(String keyword, int keyword_id)
        {
            VerifyPageIsLoaded(driver);


            int adTopCount = 0,
                 adBottomCount = 0,
                 searchCount = 0;

            driver.Navigate().GoToUrl("https://www.google.com.ua/search?source=hp&q=" + keyword);

            if (isSelectorExist(By.CssSelector(".ads-ad")))
            {//если реклама найдена
             //выбираем все блоки, которые относятся к рекламе 

                //#tvcap , #bottomads
                adTopCount = driver.FindElements(By.CssSelector("#tvcap .ads-ad")).Count;
                adBottomCount = driver.FindElements(By.CssSelector("#bottomads .ads-ad")).Count;

                if (adTopCount > 0)
                {
                    adTopCount = 0;
                    foreach (IWebElement i in driver.FindElements(By.CssSelector("#tvcap .ads-ad")))
                    {
                        ++adTopCount;
                        String localUrl = isSelectorExist(By.CssSelector("._Jwu")) ? i.FindElement(By.CssSelector("._Jwu")).Text : "";
                        String description = isSelectorExist(By.CssSelector("._WGk")) ? i.FindElement(By.CssSelector("._WGk")).Text : "";
                        //разбираем рекламное сообщение

                        if (url.Trim() != "" || description.Trim() != "")
                        {
                            int urlId = isUrlExist(localUrl);
                            if (urlId == -1)
                                Insert_Site(localUrl);

                            urlId = Select_Site_id(localUrl);

                            //формируем новую запись в бд
                            Keyword kw = new Keyword();

                            kw.keyword_id = keyword_id;
                            kw.description = description;
                            kw.is_ad = true;
                            kw.region_id = this.regionId;
                            kw.site_id = urlId;

                            kw.position = (byte)adTopCount;

                            kw.position = (byte)searchCount;
                            kw.search_engine = (byte)Keyword.SearchEngine.GOOGLE;
                            kw.created_at = DateTime.Now;
                            kw.updated_at = DateTime.Now;

                            Console.WriteLine(kw.toString());
                            if (!isExist_AdSearchPosition(kw))
                                Insert_AdSearchPosition(kw);

                        }


                    }
                }


                if (adBottomCount > 0)
                {
                    adBottomCount = 0;
                    foreach (IWebElement i in driver.FindElements(By.CssSelector("#bottomads .ads-ad")))
                    {
                        ++adBottomCount;
                        String localUrl = isSelectorExist(By.CssSelector("._Jwu")) ? i.FindElement(By.CssSelector("._Jwu")).Text : "";
                        String description = isSelectorExist(By.CssSelector("._WGk")) ? i.FindElement(By.CssSelector("._WGk")).Text : "";
                        //разбираем рекламное сообщение

                        if (url.Trim() != "" || description.Trim() != "")
                        {
                            int urlId = isUrlExist(localUrl);
                            if (urlId == -1)
                                Insert_Site(localUrl);

                            urlId = Select_Site_id(localUrl);
                            //формируем новую запись в бд
                            Keyword kw = new Keyword();

                            kw.keyword_id = keyword_id;
                            kw.description = description;
                            kw.is_ad = true;
                            kw.region_id = this.regionId;
                            kw.site_id = urlId;

                            kw.position = (byte)(4 + adBottomCount);

                            kw.position = (byte)searchCount;
                            kw.search_engine = (byte)Keyword.SearchEngine.GOOGLE;
                            kw.created_at = DateTime.Now;
                            kw.updated_at = DateTime.Now;

                            Console.WriteLine(kw.toString());
                            if (!isExist_AdSearchPosition(kw))
                                Insert_AdSearchPosition(kw);

                        }


                    }
                }



                searchCount = 0;
                foreach (IWebElement i in driver.FindElements(By.CssSelector("#res .g")))
                {
                    ++searchCount;
                    String localUrl = isSelectorExist(By.CssSelector("._Rm")) ? i.FindElement(By.CssSelector("._Rm")).Text : "";
                    String description = isSelectorExist(By.CssSelector(".st")) ? i.FindElement(By.CssSelector(".st")).Text : "";
                    //разбираем рекламное сообщение

                    if (url.Trim() != "" || description.Trim() != "")
                    {
                        int urlId = isUrlExist(localUrl);
                        if (urlId == -1)
                            Insert_Site(localUrl);

                        urlId = Select_Site_id(localUrl);

                        //формируем новую запись в бд
                        Keyword kw = new Keyword();

                        kw.keyword_id = keyword_id;
                        kw.description = description;
                        kw.is_ad = false;
                        kw.region_id = this.regionId;
                        kw.site_id = urlId;

                        kw.position = (byte)(4 + adBottomCount);

                        kw.position = (byte)searchCount;
                        kw.search_engine = (byte)Keyword.SearchEngine.GOOGLE;
                        kw.created_at = DateTime.Now;
                        kw.updated_at = DateTime.Now;

                        Console.WriteLine(kw.toString());
                        if (!isExist_AdSearchPosition(kw))
                            Insert_AdSearchPosition(kw);

                    }


                }

                //#res .g ._Rm - url
                //#res .g .st - текст


            }




        }

        public void exit()
        {
            //закрываем драйвер и закрываем браузер
            driver.Close();
            driver.Quit();
        }

        public bool isSelectorExist(By selector)
        {
            return driver.FindElements(selector).Count == 0 ? false : true;
        }
    }
}
