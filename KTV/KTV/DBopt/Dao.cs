using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV.DBopt
{
    class Dao
    {
        OleDbConnection conn = null;
        OleDbTransaction dbtran = null;
        public Dao()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            conn = null;
            dbtran = null;
        }
        public void beginTransaction()
        {
            try
            {
                if (conn == null)
                {
                    conn = DbConnection.getConnection();
                    conn.Open();
                    dbtran = conn.BeginTransaction();
                }
                else
                {
                    dbtran.Commit();
                    conn.Close();
                    conn = DbConnection.getConnection();
                    conn.Open();
                    dbtran = conn.BeginTransaction();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void endTransaction()
        {
            endTransaction(1);
        }

        //iType = 0, Rollback transaction
        //iType = 1, Commint transaction
        public void endTransaction(int iType)
        {
            try
            {
                if (conn == null)
                {
                    return;
                }
                if (iType == 0)
                {
                    dbtran.Rollback();
                }
                else
                {
                    dbtran.Commit();
                }
                conn.Close();
                conn = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet Exc_Select(string sSql, string sTable)
        {
            DataSet dataset = new DataSet();
            try
            {
                OleDbCommand command = conn.CreateCommand();
                command.Transaction = dbtran;
                command.CommandText = sSql.ToUpper();
                OleDbDataAdapter oleDA = new OleDbDataAdapter();
                oleDA.SelectCommand = command;
                oleDA.Fill(dataset, sTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataset;
        }

        public bool isExistRecords(string sSql)
        {
            bool bExist = false;
            OleDbDataReader sqlReader = null;
            try
            {
                OleDbCommand command = conn.CreateCommand();
                command.Transaction = dbtran;
                command.CommandText = sSql.ToUpper();
                sqlReader = command.ExecuteReader();
                if (sqlReader.Read())
                {
                    bExist = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    sqlReader.Close();
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            return bExist;
        }

        public int Exc_Upt_Ins_Del(string sSql)
        {
            int iRet = 0;
            try
            {
                OleDbCommand command = conn.CreateCommand();
                command.Transaction = dbtran;
                command.CommandText = sSql.ToUpper();
                iRet = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                iRet = 0;
                throw ex; ;
            }
            return iRet;
        }

    }
}
