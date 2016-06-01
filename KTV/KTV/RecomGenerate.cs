using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;


namespace KTV
{
    class RecomGenerate
    {
        public RecomGenerate()
        {
            this.getAllSongs();
            this.representeAll();
            this.calulateCosines();
            this.getRecoms();
        }

        private static ProfileLearn mProfileLearn = new ProfileLearn();
        private int recomNum = 1;
        
        public static List<Song> allSongs = new List<Song>();
        public static List<Song> recomSongs = new List<Song>();

        private float[,] repAll = new float[9,6] ;
        private float[] cosines = new float[6];

        private static String[] tempSinger = { "周杰伦", "Gala","a","b","c" };
        private static String[] tempType = { "ChineseStyle", "rock", "romantic" };
        private static String[] tempLanguage = { "Chinese", "English" };


        public void setRecomNum(int _recomNum)
        {
            this.recomNum = _recomNum;
        }

        public void getAllSongs()
        {
            MySqlConnection conn;
            MySqlCommand mySqlCommand;
            conn = Database.getMySqlCon();
            conn.Open();
            String sql = "select * from ktv_song where status = 0";
            mySqlCommand = Database.getSqlCommand(sql, conn);
            Database.getRecomSongs(mySqlCommand);
            conn.Close();
        }

        public List<Song> getRecomSongs()
        {
            return recomSongs;
        }

        //represent all songs into the float[,];
        public void representeAll()
        {
            for (int i = 0; i < allSongs.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (allSongs[i].getSinger().Equals(tempSinger[j])) repAll[j, i] = 1;
                    else repAll[j, i] = 0;
                }
                for (int j = 5; j < 7; j++)
                {
                    if ((allSongs[i].getType()).Equals(tempType[j - 5])) repAll[j, i] = 1;
                    else repAll[j, i] = 0;
                }
                for (int j = 7; j < 9; j++)
                {
                    if ((allSongs[i].getLanguage()).Equals(tempLanguage[j - 7])) repAll[j, i] = 1;
                    else repAll[j, i] = 0;
                }
            }
        }

        public float cosine(float[] A, float[] B) 
        {
            float cos = 0;
            //中间变量申明
            float add = 0;
            float modelA = 0;
            float modelB = 0;
            int lengthA = A.Length;
            int lengthB = B.Length;

            //if(lengthA != lengthB) { return cos; }
            for(int i = 0; i < lengthA; i++)
            {
                add += A[i] * B[i];
                modelA += A[i] * A[i];
                modelB += B[i] * B[i]; 
            }
            modelA = (float)Math.Sqrt(modelA);
            modelB = (float)Math.Sqrt(modelB);
            if (modelA * modelB != 0)
            {
                cos = add / (modelA * modelB);
            }
            return cos;
        }

        public void calulateCosines()
        {
            
            float[] tempRepAll = new float[9];
            float[] tempRepRes = new float[9];
            for(int i = 0; i <9; i++)
            {
                tempRepRes[i] = mProfileLearn.gettempResult()[i, 0];
            }
            for(int i = 0; i < repAll.GetLength(1); i++)
            {
                
                for(int j = 0; j < 9; j++)
                {
                    tempRepAll[j] = repAll[j, i];
                   
                }
               cosines[i] = cosine(tempRepRes,tempRepAll);
              
            }
           
        }


        /// <summary>
        /// @param tags: to tag the positions of bigger max cosines
        /// @param tempCosines: to get the tags in a easier way,doont need any sort
        /// this method : Find the recommendension,number according to the "recomNum"
        /// </summary>
        public void getRecoms()
        {
            float[] tempCosines = cosines;
            int[] tags = new int[recomNum];

            for (int i = 0; i < recomNum; i++)
            {
                float max = 0;
                int position = 0;
                for(int j = 0; j < tempCosines.Length; j++)
                {
                    if (max < tempCosines[j])
                    {
                        max = tempCosines[j];
                        position = j;
                    }
                }
                tags[i] = position;
                tempCosines[position] = 0;
            }

            for(int i = 0; i < recomNum; i++)
            {
                recomSongs.Add(allSongs[tags[i]]);
            }
        }





    }
}
