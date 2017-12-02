using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    interface WebSite
    {
        void search(String keyword,int keyword_id);
        Boolean isSelectorExist(By selector);
        void exit();
    }
}
