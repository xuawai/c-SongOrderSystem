using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;

namespace KTV
{
    public partial class Form1 : Form
    {
        private Video myVideo;
        private Form middleMenu1;
        private Form middleIndex;
        private string pathPlayPause;

        public Form1()
        {
           

            InitializeComponent();
            middleIndex = new MiddleIndex();
            Control_Add(middleIndex);
            doubleBuffer();
           
        }




        

       

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            doubleBuffer();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            middleMenu1 = new MiddleMenu1();
            Control_Add(middleMenu1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.BackgroundImage = Image.FromFile("image/background.jpg");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            doubleBuffer();
        }

       

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int height = panel3.Height;
            int width = panel3.Width;
            if (myVideo != null)
                myVideo.Dispose();
            myVideo = new Video("video/周杰伦-青花瓷.avi");
            myVideo.Owner = panel3;
            panel3.Width = width;
            panel3.Height = height;
            myVideo.Play();           
            myVideo.Pause();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (myVideo != null)
                myVideo.Audio.Volume = -1000; 
                if (pathPlayPause == "image/play.ico")
                {
                    myVideo.Play();
                    pathPlayPause = "image/stop.ico";
                    label3.Text = "暂停";
                    pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
                }
                else
                {
                    myVideo.Pause();
                    pathPlayPause = "image/play.ico";
                    label3.Text = "播放";
                    pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
                }
                
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (myVideo != null)
            {
                if(myVideo.Audio.Volume<-100)
                myVideo.Audio.Volume += 100;
                
            }
               


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (myVideo != null)
            {
                if (myVideo.Audio.Volume >-1400)
                    myVideo.Audio.Volume -= 100;

            }
        }

        private void doubleBuffer()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            doubleBuffer();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            pathPlayPause = "image/play.ico";
            pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
            doubleBuffer();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            doubleBuffer();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is MiddleIndex)
                {
                    break;
                }
　　　　　　  

                if (ctrl is MiddleMenu1 && ctrl.Controls[0] is Label)
                {
                    middleIndex = new MiddleIndex();
                    Control_Add(middleIndex);
                }
                else
                {
                    middleMenu1 = new MiddleMenu1();
                    Control_Add(middleMenu1);
                    
                }
            }
        }

        private void Control_Add(Form form)
        {
            panel1.Controls.Clear();	//移除所有控件
            form.TopLevel = false;	  //设置为非顶级窗体
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //设置窗体为非边框样式
            form.Dock = System.Windows.Forms.DockStyle.Fill;				  //设置样式是否填充整个panel
            panel1.Controls.Add(form);		//添加窗体
            form.Show();					  //窗体运行
        }

       

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
        
        }

        private void label9_Click(object sender, EventArgs e)
        {
        
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

            myVideo.Stop();

            myVideo = null;
            if (myVideo != null)
                myVideo.Dispose();
            myVideo = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            label9.Text = "时间:" + dt.ToString("t") + ":" + dt.Second;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label11.Left < -label11.Width)
                label11.Left = label11.Width;
            else
                label11.Left -= 5;
        }
         

       
    }
}
