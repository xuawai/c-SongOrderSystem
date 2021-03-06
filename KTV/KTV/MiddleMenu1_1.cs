﻿using System;
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
    public partial class MiddleMenu1_1 : Form
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

        public MiddleMenu1_1()
        {
           

            InitializeComponent();
            dataGridView1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowTemplate.Height = 30;
            
        }

        private void MiddleMenu_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleIndex.jpg");

            preAllCount = ListOfSong.songList.Count;


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


               


            allCount = dataGridView1.Rows.Count;    //获取数据表中记录的个数

            pagecount = allCount % pagesize;  

               
            if (pagecount == 0)
            {
                pagecount = allCount / pagesize;
            }
            else
            {
                pagecount = allCount / pagesize + 1;
            }

            this.label1.Text = "Total " + pagecount.ToString();

            

            show(1, pagesize);
                
               
        }

          
        private void show(int Inum,int pagesize)
        {

            String s = "select count(name) from ktv_song where name like '%" + condition + "%'";
            mySqlCommand = Database.getSqlCommand(s, conn);
            allCount = Database.getNumForQuery(mySqlCommand);
            pagecount = allCount % pagesize;
            if (pagecount == 0)
            {
                pagecount = allCount / pagesize;
            }
            else
            {
                pagecount = allCount / pagesize + 1;
            }
            this.label1.Text = "Total " + pagecount.ToString();


            
            mdap = new MySqlDataAdapter("select name,singer from ktv_song where name like '%"+condition+"%' order by id limit " + pagesize * (Inum - 1)+","+pagesize, conn);

            DataSet ds = new DataSet();  
         
            mdap.Fill(ds, "one");

            this.dataGridView1.DataSource = ds.Tables["one"].DefaultView;

            



            //在这里遍历一下数据库，根据选中字段，对已经被选择的歌曲，按钮图片做一下改变
            String sql = "select status from ktv_song where name like '%" + condition + "%' order by id limit " + pagesize * (Inum - 1) + "," + pagesize;
            mySqlCommand = Database.getSqlCommand(sql, conn);
            List<Int32> list = Database.getResultCheck(mySqlCommand);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == 0)
                {
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/uncheck.ico");
                     
                }
                else
                {
                    this.dataGridView1.Rows[i].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");
                    
                }
            }




            list = null;
            ds = null;
            this.label2.Text = "Page " + Inum.ToString() + "";
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
        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
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



       

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int CIndex = e.ColumnIndex;
            int RIndex = e.RowIndex;
            String name;
            String singer;
            //Song song = new Song();
            if (RIndex>=0)
            {


                name = this.dataGridView1.Rows[RIndex].Cells[1].Value.ToString();
                singer = this.dataGridView1.Rows[RIndex].Cells[2].Value.ToString();
                //这里将歌曲hot加一，同时加入列表
                //将数据库里相应的歌曲字段置为选中状态

                String sql = "select hot,status from ktv_song where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                int hot = Database.getResultHot(mySqlCommand);
                hot = hot + 1;

                sql = "update ktv_song set hot = " + hot + " where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateHot(mySqlCommand);

                sql = "select * from ktv_song where name = '"+name+"' and singer = '"+singer+"'";
                mySqlCommand = Database.getSqlCommand(sql,conn);
                Database.getResultset(mySqlCommand);

                sql = "update ktv_song set status = 1 where name = '" + name + "' and singer = '" + singer + "'";
                mySqlCommand = Database.getSqlCommand(sql, conn);
                Database.updateStatus(mySqlCommand);

                

                //song.setName(name);
                //song.setSinger(singer);
                //ListOfSong.songList.Add(song);
                
                this.dataGridView1.Rows[RIndex].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");

                preAllCount = ListOfSong.songList.Count;
            }
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            condition = textBox1.Text;
            show(1, pagesize);
           
        }

        private void timer1_Tick(object sender, EventArgs e)              //初始化界面，将已选择歌曲的图标置为check.ico
        {
            pictureBox1_Click(sender,e);
            timer1.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)               //当一首歌从列表中被删除，则图标置为uncheck.ico
        {
            if (preAllCount > ListOfSong.songList.Count)
            {
                preAllCount = ListOfSong.songList.Count;
                show(Inum, pagesize);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    

       

        

        
    }
}
