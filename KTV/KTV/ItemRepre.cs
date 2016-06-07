using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace KTV
{
    class ItemRepre
    {
        //capacity[0][1][2] - The Data length of Singer\Type\Language We need to consider in our Representation 
        private static int[] capacity = { RecomGenerate.singerLength, RecomGenerate.typeLength, RecomGenerate.languageLength };
        public static int arrayLength = capacity[0] + capacity[1] + capacity[2];

        protected static List<Song> songs = new List<Song>();

        protected static int[,] rep  ;

        public void setRepArray()
        {
            rep = new int[arrayLength,songs.Count];
        }

        public int[,] getRep()
        {
            return ItemRepre.rep;
        }

        public void setSongs()
        {
            songs = ListOfData.songList;
        }

        //赋值
        public void TrainData()
        {
            // Represent
            setSongs();
            represente();
        }

        

        //Todo: 测试成功后用Capacity[i] 替换以下的循环终结值
        public void represente()
        {
            setRepArray();
            int i, j;
            for (i = 0; i < rep.GetLength(1); i++)
            {
                for (j = 0; j < capacity[0]; j++)
                {
                    if ((songs[i].getSinger()).Equals(RecomGenerate.singer[j])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
                for (j = capacity[0]; j <capacity[0]+capacity[1]-1; j++)
                {
                    if ((songs[i].getType()).Equals(RecomGenerate.type[j-capacity[0]])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
                for (j = capacity[0] + capacity[1]; j < capacity[0]+capacity[1]+capacity[2] - 1; j++)
                {
                    if ((songs[i].getLanguage()).Equals(RecomGenerate.language[j - capacity[0] - capacity[1]])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
            }
        }

    }
}
