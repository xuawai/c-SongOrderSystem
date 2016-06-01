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
    public partial class MiddleMenu1_6 : Form
    {
        private int Inum = 1;
        int pagesize = 6;
        int allCount = 0;
        int pagecount = 0;

        private int preAllCount;


        private MySqlConnection conn;
        private MySqlCommand mySqlCommand;

        DataGridViewImageColumn btnImageEdit;



        public MiddleMenu1_6()
        {


            InitializeComponent();
            dataGridView1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            

        }

        private void MiddleMenu1_6_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleIndex.jpg");

            preAllCount = ListOfSong.songList.Count;

            conn = Database.getMySqlCon();
            conn.Open();

            //推荐歌曲
            RecomGenerate mrecomGenerate = new RecomGenerate();
 
     
            btnImageEdit = new DataGridViewImageColumn(false);
            btnImageEdit.Image = Image.FromFile("image/uncheck.ico");
            btnImageEdit.HeaderText = "添加";
            btnImageEdit.Name = "btnImageEdit";
            this.dataGridView1.Columns.Insert(0, btnImageEdit);
            this.dataGridView1.Columns[0].Width = 40;




            allCount = dataGridView1.Rows.Count;

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

            show(1, pagesize);


        }


        private void show(int Inum, int pagesize)
        {

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
                dataGridView1.Rows.RemoveAt(i);

            for (int i = 0; i < RecomGenerate.recomSongs.Count; i++)
            {
                DataGridViewRow Row = new DataGridViewRow();
                int index = dataGridView1.Rows.Add(Row);
                dataGridView1.Rows[i].Height = 30;
                this.dataGridView1.Rows[i].Cells[0].Value = Image.FromFile("image/uncheck.ico");
                dataGridView1.Rows[i].Cells[1].Value = RecomGenerate.recomSongs[i].getName();
                dataGridView1.Rows[i].Cells[2].Value = RecomGenerate.recomSongs[i].getSinger();
            }

            List<int> list = new List<int>();;
            for (int i = 0; i < RecomGenerate.recomSongs.Count; i++)
            {
                String sql = "select status from ktv_song where name = '" + RecomGenerate.recomSongs[i].getName() + "' and singer = '" + RecomGenerate.recomSongs[i].getSinger() + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                int status = Database.getResulStatusForOne(mySqlCommand);
                list.Add(status);
            }
            for (int i = 0; i < RecomGenerate.recomSongs.Count; i++)
            {
                if (list[i] == 0)
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/uncheck.ico");
                else
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");
            }


            list = null;
            //ds = null;
            this.label2.Text = "第" + Inum.ToString() + "页";
        }

        //首页
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum = 1;
            show(Inum, pagesize);
        }

        //尾页
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inum = pagecount;
            show(Inum, pagesize);
        }

        //上一页
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


        //下一页
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


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int CIndex = e.ColumnIndex;
            int RIndex = e.RowIndex;
            String name;
            String singer;

            if (RIndex >= 0)
            {

                name = this.dataGridView1.Rows[RIndex].Cells[1].Value.ToString();
                singer = this.dataGridView1.Rows[RIndex].Cells[2].Value.ToString();

                String sql = "select hot,status from ktv_song where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                int hot = Database.getResultHot(mySqlCommand);
                hot = hot + 1;

                sql = "update ktv_song set hot = " + hot + " where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateHot(mySqlCommand);

                sql = "select * from ktv_song where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.getResultset(mySqlCommand);

                sql = "update ktv_song set status = 1 where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateStatus(mySqlCommand);

                this.dataGridView1.Rows[RIndex].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");

                preAllCount = ListOfSong.songList.Count;
            }
        }





        private void timer1_Tick_1(object sender, EventArgs e)
        {
            show(1, pagesize);
            timer1.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (preAllCount > ListOfSong.songList.Count)
            {
                preAllCount = ListOfSong.songList.Count;
                show(Inum, pagesize);
            }
        }

        

       

       

        

        

        

       





    }
}
