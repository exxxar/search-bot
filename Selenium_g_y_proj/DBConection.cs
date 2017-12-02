using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Selenium_g_y_proj
{
    class DBConection
    {

        public class DBKeyword {
            public string keyword { get; set; }
            public int keyword_id { get; set; }
            }
        private MySqlConnection conn;

        private void Initialize()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["keywordsConnStr"]
                .ConnectionString;

            conn = new MySqlConnection(connStr);
        }
       

        public Boolean isExist(Keyword keyword)
        {
            //при проверке учитывать url и браузер
            // throw new Exception("testing");
            Console.WriteLine(keyword.url + " не найден в бд");
            return false;
        }
        //Insert statement
        public void Insert(Keyword keyword)
        {
            Console.WriteLine(keyword.url + " успешно добавлен в бд");
            //throw new Exception("testing insert");
        }

        //Update statement
        public void Update(Keyword keyword)
        {
            Console.WriteLine(keyword.url + " успешно обновлен в бд");
            //throw new Exception("testing update");
        }

        public List<DBKeyword> list(int offset,int limit)
        {
            //выбирае из бд инфу с определенным смещением, чтоб не нагружать оперативку
            return new List<DBKeyword>();
        }

        public int count()
        {
            return 0;
        }
    }
}
