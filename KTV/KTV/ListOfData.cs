using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV
{
    class ListOfData
    {
        public static List<Song> songList = new List<Song>();

        //将每首选择的歌曲加入数据列表
        public static void addData(Song song)
        {
            for (int i = 0; i < songList.Count; i++)
                if (songList[i].getName() == song.getName() && songList[i].getSinger() == song.getSinger())
                    return;
            songList.Add(song);
            return;
        }
    }
}
