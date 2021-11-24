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
            //on gère ci-dessous l'affichage des éléments pour qu'ils soient transparents
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            button2.BackColor = Color.Transparent;
            button2.Parent = pictureBox1;

        }

    

        private void button1_Click(object sender, EventArgs e)
        {
            //bouton quitter
            this.Hide();
            var form2 = new Menu();
          
            form2.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.IO.Path.GetFullPath(Environment.CurrentDirectory + @"\..\..\..\Resources\");
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               
                //récupération du nom du fichier
                Program.fichierMots = openFileDialog1.FileName;
                            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            
        }

      
    }
}
