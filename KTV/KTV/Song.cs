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
        private int status;

        public void setId(int id)
        {
            this.id = id;
        }

        public int getId()
        {
            return this.id;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public String getName()
        {
            return name;
        }

        public void setSinger(String singer)
        {
            this.singer = singer;
        }

        public String getSinger()
        {
            return singer;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public String getType()
        {
            return this.type;
        }

        public void setLanguage(String language)
        {
            this.language = language;
        }

        public String getLanguage()
        {
            return this.language;
        }

        public void setHot(int hot)
        {
            this.hot = hot;
        }

        public int getHot()
        {
            return this.hot;
        }
        public void setPath(String path)
        {
            this.path = path;
        }

        public String getPath()
        {
            return path;
        }

        public void setStatus(int _status)
        {
            this.status = _status;
        }

        public int getStatus()
        {
            return this.status;
        }
    }
}
