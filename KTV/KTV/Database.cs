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
                        song.setId(reader.GetInt32(0));
                        song.setName(reader.GetString(1));
                        song.setSinger(reader.GetString(2));
                        song.setType(reader.GetString(3));
                        song.setLanguage(reader.GetString(4));
                        song.setHot(reader.GetInt32(5));
                        song.setPath(reader.GetString(6));
                        if (reader.GetInt32(7) == 1)
                            break;
                        ListOfSong.songList.Add(song);
                        ListOfData.addData(song);

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

        //得到当前歌曲状态
        public static int getResulStatusForOne(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();

            int status = 0;


            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        status = reader.GetInt32(0);
                        

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
            return status;

        }

        //得到满足查询条件的歌曲数量
        public static int getNumForQuery(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();

            int num = 0;


            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        num = reader.GetInt32(0);


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
            return num;

        }
        //得到当前歌曲播放量
        public static int getResultHot(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();

            int hot = 0;
            

            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        hot = reader.GetInt32(0);
                        if (reader.GetInt32(1) == 1)
                            hot = hot - 1;

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
            return hot;

        }
        //更新歌曲播放量，即播放量加一
        public static void updateHot(MySqlCommand mySqlCommand)
        {
            int count = mySqlCommand.ExecuteNonQuery();
        }
        //得到待推荐的所有可能歌曲
        public static void getRecomSongs(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();
            
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        Song song = new Song();
                        song.setId(reader.GetInt32(0));
                        song.setName(reader.GetString(1));
                        song.setSinger(reader.GetString(2));
                        song.setType(reader.GetString(3));
                        song.setLanguage(reader.GetString(4));
                        song.setHot(reader.GetInt32(5));
                        song.setPath(reader.GetString(6));
                        song.setStatus(reader.GetInt32(7));
                       
                        RecomGenerate.allSongs.Add(song);
                       
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

        //得到所有歌
        public static void getAllSongs(MySqlCommand mySqlCommand)
        {
            MySqlDataReader reader = null;

            reader = mySqlCommand.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {

                        Song song = new Song();
                        song.setId(reader.GetInt32(0));
                        song.setName(reader.GetString(1));
                        song.setSinger(reader.GetString(2));
                        song.setType(reader.GetString(3));
                        song.setLanguage(reader.GetString(4));
                        song.setHot(reader.GetInt32(5));
                        song.setPath(reader.GetString(6));
                        song.setStatus(reader.GetInt32(7));

                        RecomGenerate.allSongsRegardlessOfStatus.Add(song);
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
    }
}
