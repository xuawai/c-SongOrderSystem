using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KTV
{
    class Song
    {
        private String name;
        private String singer;

        public void setName(String name)
        {
            this.name = name;
        }

        public void setSinger(String singer)
        {
            this.singer = singer;
        }

        public String getName()
        {
            return name;
        }

        public String getSinger()
        {
            return singer;
        }
    }
}
