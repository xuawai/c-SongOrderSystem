﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KTV
{
    public partial class MiddleIndex : Form
    {
        public MiddleIndex()
        {
            InitializeComponent();
        }

        private void MiddleIndex_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image/MiddleMenu1_background.jpg");
        }
    }
}