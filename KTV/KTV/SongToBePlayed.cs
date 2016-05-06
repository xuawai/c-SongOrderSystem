using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KTV
{
    public partial class SongToBePlayed : Form
    {
        private int Inum = 1;
        int pagesize = 4;
        int allCount = 0;
        int pagecount = 0;

        int preAllCount;

        public SongToBePlayed()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowTemplate.Height = 40;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (preAllCount == ListOfSong.songList.Count)
                return;
            else
            {
                preAllCount = ListOfSong.songList.Count;
                showIndex();
            }
            //if(ListOfSong.songList.Count!=0)
                //label1.Text =  ListOfSong.songList[ListOfSong.songList.Count-1].getName();
        }

        private void SongToBePlayed_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");

            


            
        }

        public void showIndex()
        {
            allCount = ListOfSong.songList.Count;    //获取数据表中记录的个数

            if (allCount != 0)
            {
                pagecount = allCount % pagesize;


                if (pagecount == 0)
                {
                    pagecount = allCount / pagesize;
                }
                else
                {
                    pagecount = allCount / pagesize + 1;
                }

                this.label1.Text = "共" + pagecount.ToString() + "页";
                if (allCount <= pagesize)
                    show(1, pagesize);
                else
                {
                  
                    show(pagecount,pagesize);
                   
                }
            }
         
        }

        private void show(int pagecount, int pagesize)
        {
            Inum = pagecount;
            int startIndex = (Inum-1)*pagesize;
            int showCount;
            if ((allCount - startIndex) <= pagesize)
                showCount = (allCount - startIndex);
            else
                showCount = 6;

            for (int i = dataGridView1.Rows.Count-1; i >=0; i--)
                     dataGridView1.Rows.RemoveAt(i);


               for (int i = 0; i < showCount; i++)
               {
                   DataGridViewRow Row = new DataGridViewRow();
                   int index = dataGridView1.Rows.Add(Row);
                   dataGridView1.Rows[i].Cells[0].Value = ListOfSong.songList[startIndex + i].getName();
                   dataGridView1.Rows[i].Cells[1].Value = ListOfSong.songList[startIndex + i].getSinger();
               }

            



            this.label2.Text = "第" + Inum.ToString() + "页";
        }

              

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void linkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum++;
            if (Inum <= pagecount)
            {
                show(Inum, pagesize);
            }
            else
            {
                MessageBox.Show("现已是最后一页记录");
                Inum = pagecount;
                return;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum = 1;
            show(Inum, pagesize);
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum = pagecount;
            show(Inum, pagesize);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum--;
            if (Inum > 0)
            {
                show(Inum, pagesize);
            }
            else
            {
                MessageBox.Show("现已是第一页记录");
                Inum = 1;
                return;
            }
        }

       



    }
}
