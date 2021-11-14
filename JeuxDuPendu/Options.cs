﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JeuxDuPendu
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            button2.BackColor = Color.Transparent;
            button2.Parent = pictureBox1;

            button3.BackColor = Color.Transparent;
            button3.Parent = pictureBox1;
        }

    

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.IO.Path.GetFullPath(Environment.CurrentDirectory + @"\..\..\..\Resources\");
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               // Stream stream = openFileDialog1.OpenFile();
                //TODO lire le fichier
                Program.fichierMots = openFileDialog1.FileName;
                            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = System.IO.Path.GetFullPath(Environment.CurrentDirectory + @"\..\..\..\Resources\");
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
               // Stream stream = openFileDialog2.OpenFile();
                //TODO lire le fichier
                Program.fichierSauvegarde = openFileDialog2.FileName;
            }
        }
    }
}
