using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Selenium_g_y_proj
{
    class DBConection
    {
        public class DBConnectionException:Exception
        {

        }
        public class DBKeyword
        {
            public string keyword { get; set; }
            public int keyword_id { get; set; }
        }
        private MySqlConnection conn;

        public DBConection()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["keywordsConnStr"]
                .ConnectionString;
            conn = new MySqlConnection(connStr);
        }


        public Boolean isExist_AdSearchPosition(Keyword keyword)
        {
            if (!this.OpenConnection())
                throw new DBConnectionException();

            Console.WriteLine("Проверяем в бд [" + keyword.toString()+"]");

            int Count = -1;

            try
            {
                string query = "SELECT Count(*) as count FROM `adsearchpostions` WHERE `Keywords_id`=@Keywords_id and `search_engine`=@search_engine and `positions`=@positions and `description`=\"@description\"";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keywords_id", keyword.keyword_id);
                cmd.Parameters.AddWithValue("@search_engine", keyword.search_engine);
                cmd.Parameters.AddWithValue("@positions", keyword.position);
                cmd.Parameters.AddWithValue("@description", keyword.description);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Count = dataReader.GetInt32("count");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();
            Console.WriteLine("Кол-во в БД!![" + Count+"]");
            return Count > 0;
        }

        public void Insert_AdSearchPosition(Keyword keyword)
        {

            if (!this.OpenConnection())
                throw new DBConnectionException();

            try
            {
                string replacement = "";
                Regex rgx = new Regex("['\"]");

                Console.WriteLine("Добавляем в бд [" + keyword.toString()+"]");
                string query = "INSERT INTO `adsearchpostions` " +
                    "(`AdSearchPostions_site_id`, `description`, `positions`, `search_engine`, `Keywords_id`,`created_at`,`updated_at`,`is_ad`,`region_id`) VALUES " +
                    "(@AdSearchPostions_site_id,@description,@positions,@search_engine,@Keywords_id,@created_at,@updated_at,@is_ad,@region_id)";


                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AdSearchPostions_site_id", keyword.site_id);
                cmd.Parameters.AddWithValue("@description", rgx.Replace(keyword.description, replacement));
                cmd.Parameters.AddWithValue("@positions", keyword.position);
                cmd.Parameters.AddWithValue("@search_engine", keyword.search_engine);
                cmd.Parameters.AddWithValue("@Keywords_id", keyword.keyword_id);
                cmd.Parameters.AddWithValue("@created_at", keyword.created_at);
                cmd.Parameters.AddWithValue("@updated_at", keyword.updated_at);
                cmd.Parameters.AddWithValue("@is_ad", keyword.is_ad);
                cmd.Parameters.AddWithValue("@region_id", keyword.region_id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();
        }



        public int isUrlExist(String site_url)
        {
            if (!this.OpenConnection())
                return -1;

            int site_id = -1;

            try
            {

                string query = "SELECT * FROM `site` WHERE `site`=@site_url;";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@site_url", site_url);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    site_id = dataReader.GetInt32("site_id");
                }

                dataReader.Close();
                this.CloseConnection();
                return site_id;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return site_id;
        }

        public void Insert_Site(String site_url)
        {
            if (!this.OpenConnection())
                return;

            Console.WriteLine("Добавляем URL в бд " + site_url);

            //create command and assign the query and connection from the constructor
            try
            {
                string query = "INSERT INTO `site` (`site`) VALUES ( @site_url );";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@site_url", site_url);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();
        }

        public void Insert_Uri(String site_uri)
        {
            if (!this.OpenConnection())
                return;

            //create command and assign the query and connection from the constructor
            try
            {
                string query = "INSERT INTO `uri` (`uri`,`site_id`) VALUES (@uri,@site_id);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uri", site_uri);
                cmd.Parameters.AddWithValue("@site_id", site_uri);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();
        }

        public int Select_Uri_id(String uri)
        {

            if (!this.OpenConnection())
                return -1;

            int uri_id = -1;

            try
            {
                //Create Command
                string query = "SELECT uri_id FROM `uri` WHERE `uri`=@uri";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uri", uri);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    uri_id = dataReader.GetInt32("uri_id");
                }
                dataReader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();

            return uri_id;
        }


        public int Select_Site_id(String site_url)
        {

            if (!this.OpenConnection())
                return -1;

            int site_id = -1;

            try
            {
                //Create Command
                string query = "SELECT site_id FROM `site` WHERE `site`=@site_url";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@site_url", site_url);

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    site_id = dataReader.GetInt32("site_id");
                }
                dataReader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            this.CloseConnection();

            return site_id;
        }


        public List<DBKeyword> list(int offset, int limit)
        {
            if (!this.OpenConnection())
                return new List<DBKeyword>();

            //выбирае из бд инфу с определенным смещением, чтоб не нагружать оперативку
            string query = "SELECT `keyword`, `id` FROM `keywords` LIMIT @limit OFFSET @offset;";
            List<DBKeyword> list_kw = new List<DBKeyword>();

            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@limit", limit);
            cmd.Parameters.AddWithValue("@offset", offset);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                DBKeyword dbkw = new DBKeyword();
                dbkw.keyword = "" + dataReader["keyword"];
                dbkw.keyword_id = Int32.Parse("" + dataReader["id"]);
                list_kw.Add(dbkw);
            }

            dataReader.Close();
            this.CloseConnection();

            return list_kw;
        }

        public int count()
        {
            if (!this.OpenConnection())
                return -1;

            string query = "SELECT Count(*) FROM `keywords`";
            int Count = -1;

            MySqlCommand cmd = new MySqlCommand(query, conn);
            Count = int.Parse(cmd.ExecuteScalar() + "");
            this.CloseConnection();

            return Count;

        }

        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {

                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
    }
}
