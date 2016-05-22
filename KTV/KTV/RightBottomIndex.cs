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
    public partial class RightBottomIndex : Form
    {
        public RightBottomIndex()
        {
            InitializeComponent();
        }

        private void RightBottomIndex_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/SongList.jpg");
        }
    }
}
