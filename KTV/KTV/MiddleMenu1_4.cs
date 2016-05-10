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
    public partial class MiddleMenu1_4 : Form
    {
        private int Inum = 1; 
        int pagesize = 6;
        int allCount = 0;
        int pagecount = 0;

        private int preAllCount;


        private MySqlConnection conn;
        private MySqlDataAdapter mdap;
        private MySqlCommand mySqlCommand;

        DataGridViewImageColumn btnImageEdit;

        private String condition;

        public MiddleMenu1_4()
        {
           

            InitializeComponent();
            dataGridView1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowTemplate.Height = 30;
            
        }

        private void MiddleMenu1_4_Load_1(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");


            conn = Database.getMySqlCon();
            conn.Open();
            mdap = new MySqlDataAdapter("select name,singer from ktv_song", conn);
            DataSet ds = new DataSet();
            mdap.Fill(ds, "one");
            this.dataGridView1.DataSource = ds.Tables["one"].DefaultView;


                btnImageEdit = new DataGridViewImageColumn(false);
                btnImageEdit.Image = Image.FromFile("image/uncheck.ico");                
                btnImageEdit.HeaderText = "添加";
                btnImageEdit.Name = "btnImageEdit";
                this.dataGridView1.Columns.Insert(0, btnImageEdit);


               


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

          
        private void show(int Inum,int pagesize)
        {
            
            mdap = new MySqlDataAdapter("select name,singer from ktv_song where type like '%"+condition+"%' order by id limit " + pagesize * (Inum - 1)+","+pagesize, conn);
            DataSet ds = new DataSet();          
            mdap.Fill(ds, "one");
            this.dataGridView1.DataSource = ds.Tables["one"].DefaultView;


            String sql = "select status from ktv_song where type like '%" + condition + "%' order by id limit " + pagesize * (Inum - 1) + "," + pagesize;
            mySqlCommand = Database.getSqlCommand(sql, conn);
            List<Int32> list = Database.getResultCheck(mySqlCommand);
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i]==0)
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/uncheck.ico");
                else
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");
            }


            list = null;
            ds = null;
            this.label2.Text = "第" + Inum.ToString() + "页";
        }

        //首页
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

                String sql = "select * from ktv_song where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.getResultset(mySqlCommand);

                sql = "update ktv_song set status = 1 where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateStatus(mySqlCommand);

                this.dataGridView1.Rows[RIndex].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");

                preAllCount = ListOfSong.songList.Count;
            }
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            condition = comboBox1.Text;
            show(1, pagesize);
           
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            pictureBox1_Click(sender,e);
            timer1.Enabled = false;
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (preAllCount > ListOfSong.songList.Count)
            {
                preAllCount = ListOfSong.songList.Count;
                pictureBox1_Click(sender, e);
            }
        }

        


       
    }
}
