using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KTV
{
    public partial class SongToBePlayed : Form
    {
        private int Inum = 1;
        int pagesize = 4;
        int allCount = 0;
        int pagecount = 0;
        int preAllCount = 0;

        private MySqlConnection conn;
        private MySqlDataAdapter mdap;
        private MySqlCommand mySqlCommand;

        DataGridViewImageColumn btnImageTop;
        DataGridViewImageColumn btnImageDelete;

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
            else if (preAllCount < ListOfSong.songList.Count)
            {
                preAllCount = ListOfSong.songList.Count;
                showIndex(1);//增添
            }
            else if (preAllCount > ListOfSong.songList.Count)
            {
                preAllCount = ListOfSong.songList.Count;
                showIndex(2);//删除
            }
           
        }

        private void SongToBePlayed_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");

            conn = Database.getMySqlCon();
            conn.Open();


            btnImageTop = new DataGridViewImageColumn(false);
            btnImageTop.Image = Image.FromFile("image/top.ico");
            btnImageTop.HeaderText = "置顶";
            btnImageTop.Name = "btnImageTop";           
            this.dataGridView1.Columns.Insert(2, btnImageTop);

            this.dataGridView1.Columns[2].Width = 32;


            btnImageDelete = new DataGridViewImageColumn(false);
            btnImageDelete.Image = Image.FromFile("image/delete.ico");
            btnImageDelete.HeaderText = "置顶";
            btnImageDelete.Name = "btnImageDelete";
            this.dataGridView1.Columns.Insert(3, btnImageDelete);

            this.dataGridView1.Columns[3].Width = 32;
            
        }

        public void showIndex(int type)
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
                if (type == 1)
                    show(pagecount, pagesize);
                else if (type == 2)
                {
                    if (allCount % pagesize == 0 && Inum==pagecount+1)
                        show(pagecount, pagesize);
                    else
                        show(Inum,pagesize);
                }
                else if (type == 3)
                    show(Inum, pagesize);
                   
                
            }
            else
            {
                this.label1.Text = "共0页";
                show(1, 0);
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
                showCount = pagesize;

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

     

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int CIndex = e.ColumnIndex;
            int RIndex = e.RowIndex;
            int index = (Inum - 1) * pagesize + RIndex;
            String name;
            String singer;
            //删除
            if (CIndex == 3)
            {
                name = this.dataGridView1.Rows[RIndex].Cells[0].Value.ToString();
                singer = this.dataGridView1.Rows[RIndex].Cells[1].Value.ToString();

                //从列表中删去        
                ListOfSong.songList.RemoveAt(index);

                //从数据库中把相应歌曲设置为未选择状态
                String sql = "update ktv_song set status = 0 where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateStatus(mySqlCommand);
            }
            //置顶
            else
            {
                Song song = new Song();
                song = ListOfSong.songList[index];
                //从列表中删去        
                ListOfSong.songList.RemoveAt(index);
                //置顶       
                ListOfSong.songList.Insert(0,song);
                showIndex(3);
            }
        }

      

       



    }
}
