using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;
using MySql.Data.MySqlClient;

namespace KTV
{
    public partial class Form1 : Form
    {
        private Video myVideo;
        private Form middleMenu1;
        private Form middleIndex;
        private Form songToBePlayed;
        private Form rightBottomIndex;
        private string pathPlayPause;
        private int firstSongPlayed;        //等于零，表示歌单为空或者第一首歌还未播放
        private bool isrightBottomIndex = true;   //等于真，代表右下panel首次载入

        private int volume = -1000;
        private double lastCurrentPosition = -1;
        private double latestCurrentPosition = 0;
        private Song currentSong;

        private MySqlConnection conn;
        private MySqlCommand mySqlCommand;
        public Form1()
        {
           

            InitializeComponent();

            middleIndex = new MiddleIndex();
            rightBottomIndex = new RightBottomIndex();
            songToBePlayed = new SongToBePlayed();
            
            Control_Add(middleIndex);

            Control_Add2(rightBottomIndex);
            
            currentSong = new Song();

            conn = Database.getMySqlCon();
            conn.Open();

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
            if (isrightBottomIndex)
            {
                 Control_Add2(songToBePlayed);
                 isrightBottomIndex = false;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.BackgroundImage = Image.FromFile("image/background.jpg");
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            doubleBuffer();
        }

       

        private void playCurrentSong(object sender, EventArgs e)   //播放当前列表第一首歌
        {
            ;

            int height = panel3.Height;
            int width = panel3.Width;
            if (myVideo != null)
                myVideo.Dispose();
            String path = ListOfSong.songList[0].getPath();
            myVideo = new Video(path);
            myVideo.Owner = panel3;
            panel3.Width = width;
            panel3.Height = height;
            myVideo.Audio.Volume = volume;
            if (ListOfSong.songList.Count >= 2)
                label11.Text = "当前播放："+ListOfSong.songList[0].getName() + "-" + ListOfSong.songList[0].getSinger() + "   下曲：" +
                               ListOfSong.songList[1].getName() + "-" + ListOfSong.songList[1].getSinger();
            else
                label11.Text = "当前播放：" + ListOfSong.songList[0].getName() + "-" + ListOfSong.songList[0].getSinger() + "   下曲：暂无";

            currentSong.setName(ListOfSong.songList[0].getName());
            currentSong.setSinger(ListOfSong.songList[0].getSinger());
            currentSong.setPath(ListOfSong.songList[0].getPath());

            //从数据库中把相应歌曲设置为未选择状态
            String sql = "update ktv_song set status = 0 where name = '" + currentSong.getName() + "' and singer = '" + currentSong.getSinger() + "'";
            mySqlCommand = Database.getSqlCommand(sql, conn);
            Database.updateStatus(mySqlCommand);

            ListOfSong.songList.RemoveAt(0);                  //从列表中删除当前歌曲

           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
                
                if (firstSongPlayed == 0)//表示歌单为空或者第一首歌还未播放
                {
                    if( ListOfSong.songList.Count != 0)
                    {
                          firstSongPlayed = 1;//表示第一首歌已经开始播放且歌单未播放完
                          playCurrentSong(sender, e);                         
                          myVideo.Play();
                          pathPlayPause = "image/stop.ico";
                          label3.Text = "暂停";
                          pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
                    }
                    else 
                        return;
                }
                else
                {
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
            
                
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (myVideo != null)
            {
                if (volume < -100)
                {
                    volume += 100;
                    myVideo.Audio.Volume = volume;
                }
                
                
            }
               


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (myVideo != null)
            {
                if (volume > -1400)
                {
                    volume -= 100;
                    myVideo.Audio.Volume = volume;
                }
                    

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

        private void Control_Add2(Form form)
        {
            panel4.Controls.Clear();	//移除所有控件
            form.TopLevel = false;	  //设置为非顶级窗体
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //设置窗体为非边框样式
            form.Dock = System.Windows.Forms.DockStyle.Fill;				  //设置样式是否填充整个panel
            panel4.Controls.Add(form);		//添加窗体
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

        private void pictureBox7_Click(object sender, EventArgs e)   //切歌
        {
            if (pathPlayPause == "image/stop.ico")
                myVideo.Stop();

            myVideo = null;
            if (myVideo != null)
                myVideo.Dispose();
            myVideo = null;

            if (pathPlayPause == "image/stop.ico")
            {
                pathPlayPause = "image/play.ico";
                label3.Text = "播放";
                pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
            }

            playNextSong(sender,e);
        }

        private void timer1_Tick(object sender, EventArgs e)  //显示时间
        {
            DateTime dt = DateTime.Now;
            label9.Text = "时间:" + dt.ToString("t") + ":" + dt.Second;
        }

        private void timer2_Tick(object sender, EventArgs e)  //显示滚动条
        {
            if (firstSongPlayed == 0)
                label11.Text = "当前播放:暂无   下曲：暂无";
            else
            {
                if (ListOfSong.songList.Count >= 1)
                    label11.Text = "当前播放：" + currentSong.getName() + "-" + currentSong.getSinger() + "   下曲：" +
                                    ListOfSong.songList[0].getName() + "-" + ListOfSong.songList[0].getSinger();
                else
                    label11.Text = "当前播放：" + currentSong.getName() + "-" + currentSong.getSinger() + "   下曲：暂无";

            }
            if (label11.Left < -label11.Width)
                label11.Left = panel5.Width;
            else
                label11.Left -= 5;
        }

        private void timer3_Tick(object sender, EventArgs e)   //判断当前歌曲是否播放完毕
        {
            if (myVideo != null && pathPlayPause == "image/stop.ico")
            {
                latestCurrentPosition = myVideo.CurrentPosition;
                if (latestCurrentPosition == lastCurrentPosition)//表示当前歌曲播放完成
                {       
                    if (myVideo != null)                              //释放当前歌曲资源
                        myVideo.Dispose();
                    
                    playNextSong(sender,e);
                   
                }
                else
                {
                    lastCurrentPosition = latestCurrentPosition;
                }
                
            }
        }

        public void playNextSong(object sender, EventArgs e)   //手动或自动切歌时，播放下一首歌曲
            {
                



                    if (ListOfSong.songList.Count != 0)                    //如果歌曲列表未播放完
                    {
                        playCurrentSong(sender, e);                      //播放下一首歌曲
                        lastCurrentPosition = -1;
                        latestCurrentPosition = 0;
                        myVideo.Play();
                        if (pathPlayPause == "image/play.ico")
                        {
                            pathPlayPause = "image/stop.ico";
                            label3.Text = "暂停";
                            pictureBox2.BackgroundImage = Image.FromFile(pathPlayPause);
                        }
                    }
                    else                                                //如果歌曲列表已经为空
                    {
                        firstSongPlayed = 0;
                    }
            
            }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (currentSong.getName() != null)
            {
                ListOfSong.songList.Insert(0, currentSong);
                playNextSong(sender, e);
            }
        }
        
         

       
    }
}
