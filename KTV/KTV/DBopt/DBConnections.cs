using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV.DBopt
{
    class DBConnections
    {
        private static OleDbConnection conn = null;

        public DbConnection()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            //initConnection();
        }

        public static void initConnection()
        {
            try
            {
                conn = new OleDbConnection(DbConfig.getConnString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OleDbConnection getConnection()
        {
            initConnection();
            return conn;
        }

    }
}
