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
       

        public Boolean isExist(Keyword keyword)
        {
            string query = "SELECT Count(*) FROM `adpostions` WHERE `Keywords_id`="+keyword.keyword_id;
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();
                if (Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

        }
        //Insert statement
        public void Insert(Keyword keyword)
        {
            string query = "INSERT INTO `adpostions`(`url`, `description`, `positions`, `browser`, `Keywords_id`) VALUES ('"
                +keyword.url+"','"
                +keyword.description+"','"
                +keyword.getConcatPositions()+"',"
                +keyword.browser+","
                +keyword.keyword_id+")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
            
        }

        //Update statement
        public void Update(Keyword keyword)
        {
            string query = "UPDATE `adpostions` SET `url`='"+keyword.url
                + "',`description`='" + keyword.description
                + "',`position`='"+keyword.getConcatPositions()
                + "',`browser`="+keyword.browser
                + ",`Keywords_id`="+keyword.keyword_id  
                + "WHERE `id`="+keyword.id;


            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = conn;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public List<DBKeyword> list(int offset,int limit)
        {
            //выбирае из бд инфу с определенным смещением, чтоб не нагружать оперативку
            string query = "SELECT `keyword`, `keyword_id` FROM `keywords` LIMIT " + limit+" OFFSET "+offset+";";
            List<DBKeyword> list_kw = new List<DBKeyword>();
            
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {

                    DBKeyword dbkw = new DBKeyword();
                    dbkw.keyword = "" + dataReader["keyword"];
                    dbkw.keyword_id = Int32.Parse("" + dataReader["keyword_id"]);

                    list_kw.Add(dbkw);
                 
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list_kw;
            }
            else
            {
                return list_kw;
            }
        }

        public int count()
        {
            string query = "SELECT Count(*) FROM `keywords`";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }

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
