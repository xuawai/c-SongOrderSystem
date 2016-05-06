using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace KTV
{
    public partial class MiddleMenu1_1 : Form
    {
        private int Inum = 1; 
        int pagesize = 6;
        int allCount = 0;
        int pagecount = 0;

        private DataSet dsall;
        private static String mysqlcon = "database=gogo_website;Password=;User ID=root;server=127.0.0.1";
        private MySqlConnection conn;
        private MySqlDataAdapter mdap;

        DataGridViewImageColumn btnImageEdit;

        public MiddleMenu1_1()
        {
           

            InitializeComponent();
            dataGridView1.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.RowTemplate.Height = 30;
            
        }

        private void MiddleMenu_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");
           

           
              
                conn = new MySqlConnection(mysqlcon);
                mdap = new MySqlDataAdapter("select * from go_admin",conn);
                dsall = new DataSet();
                mdap.Fill(dsall,"go_admin");
                dataGridView1.DataSource = dsall.Tables["go_admin"];


                btnImageEdit = new DataGridViewImageColumn(false);
                // Image imgEdit = new Bitmap(Properties.Resources.Save, new Size(16, 16));
                // btnImageEdit.Image = imgEdit;
                btnImageEdit.Image = Image.FromFile("image/uncheck.ico");
                //btnImageEdit.Width = 50;
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

                this.label1.Text = "共" + pagecount.ToString() + "页";

                show(1, pagesize);                                                              

              
        }

          
        private void show(int Inum,int pagesize)
        {
            

           // mdap = new MySqlDataAdapter("select top "+pagesize+" * from go_admin where id not in (select top "+pagesize*(Inum-1)+" id from go_admin)",conn);
            mdap = new MySqlDataAdapter("select id,username from go_admin order by id limit " + pagesize * (Inum - 1)+","+pagesize, conn);

            DataSet ds = new DataSet();  
 
            
            mdap.Fill(ds, "one");

            this.dataGridView1.DataSource = ds.Tables["one"].DefaultView;


            //在这里遍历一下数据库，根据选中字段，对已经被选择的歌曲，按钮图片做一下改变
           




            
            ds = null;
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
            Song song = new Song();
            if (RIndex>=0)
            {


                name = this.dataGridView1.Rows[RIndex].Cells[1].Value.ToString();
                singer = this.dataGridView1.Rows[RIndex].Cells[2].Value.ToString();
                //这里将歌曲加入列表
                //将数据库里相应的歌曲字段置为选中状态

                song.setName(name);
                song.setSinger(singer);
                ListOfSong.songList.Add(song);
                this.dataGridView1.Rows[RIndex].Cells["btnImageEdit"].Value = Image.FromFile("image/check.ico");

                
            }
        }  

       

        

        
    }
}
