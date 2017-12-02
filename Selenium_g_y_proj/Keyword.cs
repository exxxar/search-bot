using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    class Keyword
    {
        public enum Browser
        {
            GOOGLE = 0,
            YANDEX = 1
        }

        public int id { get; set; }
        public int keyword_id { get; set; }
        public String url { get; set; }
        public String description { get; set; }
        public byte[] position { get; set; }
        public Browser browser { get; set; }

        public Keyword()
        {
            this.position = new byte[5];
        }

        public Keyword(int p_size)
        {
            this.position = new byte[p_size>0?p_size:0];
        }

    }
}
