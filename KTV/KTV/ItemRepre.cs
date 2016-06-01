using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KTV
{
    class ItemRepre
    {
        //capacity[0][1][2] - The Data length of Singer\Type\Language We need to consider in our Representation 
        private static int[] capacity = { 5, 2, 2 };

        protected static List<Song> songs = new List<Song>();

        private static String[] tempSinger = { "周杰伦", "Gala", "a", "b", "c" };
        private static String[] tempType = { "ChineseStyle", "rock", "romantic" };
        private static String[] tempLanguage = { "Chinese", "English" };

        protected int[,] rep = new int[9,5] ;

        public int[,] getRep()
        {
            return this.rep;
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
            for (int i = 0; i < songs.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((songs[i].getSinger()).Equals(tempSinger[j])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
                for (int j = 5; j < 7; j++)
                {
                    if ((songs[i].getType()).Equals(tempType[j - 5])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
                for (int j = 7; j < 9; j++)
                {
                    if ((songs[i].getLanguage()).Equals(tempLanguage[j - 7])) rep[j, i] = 1;
                    else rep[j, i] = 0;
                }
            }
        }

    }
}
