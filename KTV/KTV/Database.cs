using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace KTV
{
    class Database
    {

        public static MySqlConnection getMySqlCon()
        {
            String mysqlStr = "database=ktv_project;Password=;User ID=root;server=127.0.0.1;charset=gbk";
            MySqlConnection mysql = new MySqlConnection(mysqlStr);
            return mysql;
        }

        public static MySqlCommand getSqlCommand(String sql, MySqlConnection mysql)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mysql);
            return mySqlCommand;
        }

       //得到歌曲名与歌手
        public static void getResultset(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;
           
            reader = mySqlCommand.ExecuteReader();
            

            Song song = new Song();
            
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                       
                        song.setName(reader.GetString(1));
                        song.setSinger(reader.GetString(2));
                        song.setPath(reader.GetString(6));
                        if (reader.GetInt32(7) == 1)
                            break;
                        ListOfSong.songList.Add(song);
                       
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("查询失败了！");
            }
            finally
            {
                reader.Close();
            }
        }
        //查找歌曲状态，即是否被选择，以确认图标
        public static List<Int32> getResultCheck(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();

            List<Int32> list = new List<Int32>();
            Song song = new Song();

            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        list.Add(reader.GetInt32(0));
                    
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("查询失败了！");
            }
            finally
            {
                reader.Close();
            }
            return list;

        }
        //更新歌曲状态，即是否被选择进入播放列表
        public static void updateStatus(MySqlCommand mySqlCommand)
        {
            int count = mySqlCommand.ExecuteNonQuery();
        }
    }
}
