using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace KTV
{
    class RecomGenerate
    {
        public RecomGenerate()
        {
            clearRecomStatics();
            allSongs = new List<Song>();
            allSongsRegardlessOfStatus = new List<Song>();
            recomSongs = new List<Song>();
            this.getAllSongs();
            this.setTypeList();
            this.representeAll();
            ProfileLearn mProfileLearn = new ProfileLearn();
            this.calulateCosines();
            this.getRecoms();
        }

      //  private static ProfileLearn mProfileLearn = new ProfileLearn();
        private int recomNum = 2;
        
        public static List<Song> allSongs = new List<Song>();
        public static List<Song> allSongsRegardlessOfStatus = new List<Song>();
        public static List<Song> recomSongs = new List<Song>();

        public static List<String> singer = new List<String>();
        public static List<String> type = new List<String>();
        public static List<String> language = new List<String>();
        public static int singerLength;
        public static int typeLength;
        public static int languageLength;


        private int[,] repAll ;
        private float[] cosines ;

        public void setRepAllArray()
        {
            repAll = new int[singerLength+typeLength+languageLength,allSongs.Count];
        }

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

            String sql2 = "select * from ktv_song";
            mySqlCommand = Database.getSqlCommand(sql2, conn);
            Database.getAllSongs(mySqlCommand);

            conn.Close();
        }

        public void setTypeList()
        {
            String temp = null;
            int i,j;
            for ( i = 0; i < allSongsRegardlessOfStatus.Count; i++)
            {
                temp = allSongsRegardlessOfStatus[i].getSinger();
                for ( j = 0; j < singer.Count; j++)
                {
                    if (singer[j].Equals(temp))
                        break;
                }
                if (j == singer.Count)
                    singer.Add(temp);

                temp = allSongsRegardlessOfStatus[i].getLanguage();
                for (j = 0; j < language.Count; j++)
                {
                    if (language[j].Equals(temp))
                        break;
                }
                if (j == language.Count)
                    language.Add(temp);

                temp = allSongsRegardlessOfStatus[i].getType();
                for (j = 0; j < type.Count; j++)
                {
                    if (type[j].Equals(temp))
                        break;
                }
                if (j == type.Count)
                    type.Add(temp);
            }
            singerLength = singer.Count;
            typeLength = type.Count;
            languageLength = language.Count;

        }


        public List<Song> getRecomSongs()
        {
            return recomSongs;
        }

        //represent all songs into the float[,];
        public void representeAll()
        {
            setRepAllArray();
            cosines = new float[allSongs.Count];
            int i, j;
            for (i = 0; i < repAll.GetLength(1); i++)
            {
                for (j = 0; j < singerLength; j++)
                {
                    if ((allSongs[i].getSinger()).Equals(RecomGenerate.singer[j])) repAll[j, i] = 1;
                    else repAll[j, i] = 0;
                }
                for (j = singerLength; j < singerLength+typeLength-1; j++)
                {
                    if ((allSongs[i].getType()).Equals(RecomGenerate.type[j-singerLength])) repAll[j, i] = 1;
                    else repAll[j, i] = 0;
                }
                for (j = singerLength + typeLength; j < singerLength + typeLength+languageLength - 1; j++)
                {
                    if ((allSongs[i].getLanguage()).Equals(RecomGenerate.language[j-singerLength-typeLength])) repAll[j, i] = 1;
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
            int arrayLength = singerLength + typeLength + languageLength;
            float[] tempRepAll = new float[arrayLength];
            float[] tempRepRes = new float[arrayLength];

           
 
            for (int i = 0; i < arrayLength; i++)
            {
                //tempRepRes[i] = mProfileLearn.gettempResult()[i, 0];
                tempRepRes[i] = ProfileLearn.tempResult[i,0];
        
            }
            for(int i = 0; i < repAll.GetLength(1); i++)
            {

                for (int j = 0; j < arrayLength; j++)
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

        public void clearRecomStatics()
        {
            RecomGenerate.allSongs = null;
            RecomGenerate.allSongsRegardlessOfStatus = null;
            RecomGenerate.recomSongs = null;

        }



    }
}
