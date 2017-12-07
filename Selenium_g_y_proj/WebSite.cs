using OpenQA.Selenium;
using System;

namespace Selenium_g_y_proj
{
    interface WebSite
    {
        void search(String keyword,int keyword_id);
        Boolean isSelectorExist(By selector);
        void open_settings();
        void exit();
    }
}
