using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JeuxDuPendu
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            label1.BackColor = Color.Transparent;
            label1.Parent = pictureBox1;

            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            button2.BackColor = Color.Transparent;
            button2.Parent = pictureBox1;

            button3.BackColor = Color.Transparent;
            button3.Parent = pictureBox1;

            button4.BackColor = Color.Transparent;
            button4.Parent = pictureBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*this.Hide();
            var form2 = new GameForm();
            form2.Closed += (s, args) => this.Close();
            form2.Show();*/
            this.Hide();
            var form2 = new GameForm();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Options();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new ServerListForm();
            /*var form3 = new GameForm();
            form2.Closed += (s, args) => this.Close();
            form3.Closed += (s, args) => this.Close();
            form2.Show();*/
            form2.ShowDialog();
            this.Close();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            var client = new AsyncClient(0);
            client.StartClient();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Scores();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }
    }
}
