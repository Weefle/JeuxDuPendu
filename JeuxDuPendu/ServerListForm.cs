﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuxDuPendu
{
    public partial class ServerListForm : Form
    {
        public ServerListForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }
    }
}
