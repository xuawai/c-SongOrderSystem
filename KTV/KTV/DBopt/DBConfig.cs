using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV.DBopt
{
    class DBConfig
    {
        private static string connString = "";

        public DBConfig()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            //initConnString();
        }

        private static void initConnString()
        {
            try
            {
                connString = "Provider=SQLOLEDB;Data Source=PC201507210855\\SQLEXPRESS;Initial Catalog=Dataq;User ID=sa;Password=sa123";
                //connString = "Provider=MySQL Provider;Data Sourcr=localhost;User ID=root;Password='';Initial Catalog=";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getConnString()
        {
            initConnString();
            return connString;
        }
    }
}
