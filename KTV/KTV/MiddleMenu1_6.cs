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
    public partial class MiddleMenu1_6 : Form
    {
        public MiddleMenu1_6()
        {
            InitializeComponent();
        }

        private void MiddleMenu1_6_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");
        }
    }
}
