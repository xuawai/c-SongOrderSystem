using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV
{
    class Song
    {
        private int id;
        private String name;
        private String singer;
        private String type;
        private String language;
        private int hot;
        private String path;
        private int check;

        public void setName(String name)
        {
            this.name = name;
        }

        public void setSinger(String singer)
        {
            this.singer = singer;
        }

        public void setPath(String path)
        {
            this.path = path;
        }

        public String getName()
        {
            return name;
        }

        public String getSinger()
        {
            return singer;
        }

        

        public String getPath()
        {
            return path;
        }
    }
}
