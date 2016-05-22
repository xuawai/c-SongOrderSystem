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
    public partial class MiddleMenu1 : Form
    {
        private Form middleMenu1_1;
        private Form middleMenu1_2;
        private Form middleMenu1_3;
        private Form middleMenu1_4;
        private Form middleMenu1_5;
        private Form middleMenu1_6;

        public MiddleMenu1()
        {
            
            InitializeComponent(); 
            //双缓冲技术 必加
            doubleBuffer();
            
        }

        private void doubleBuffer()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            middleMenu1_1 = new MiddleMenu1_1();
            Control_Add(middleMenu1_1);
        }

 

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            middleMenu1_6 = new MiddleMenu1_6();
            Control_Add(middleMenu1_6);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            middleMenu1_5 = new MiddleMenu1_5();
            Control_Add(middleMenu1_5);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            middleMenu1_4 = new MiddleMenu1_4();
            Control_Add(middleMenu1_4);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            middleMenu1_3 = new MiddleMenu1_3();
            Control_Add(middleMenu1_3);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            middleMenu1_2 = new MiddleMenu1_2();
            Control_Add(middleMenu1_2);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void MiddleMenu1_Load(object sender, EventArgs e)
        {

            this.BackgroundImage = Image.FromFile("image/MiddleIndex.jpg");
            
        }


        private void Control_Add(Form form)
        {
            this.Controls.Clear();	//移除所有控件
            form.TopLevel = false;	  //设置为非顶级窗体
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //设置窗体为非边框样式
            form.Dock = System.Windows.Forms.DockStyle.Fill;				  //设置样式是否填充整个panel
            this.Controls.Add(form);		//添加窗体
            form.Show();					  //窗体运行
        }

        //非常重要的代码！！
        //保证窗口控件可以同时被加载,但是会造成加载延迟。
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
