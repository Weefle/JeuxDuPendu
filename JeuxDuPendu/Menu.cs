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
        }

        private void button1_Click(object sender, EventArgs e)
        {
     
            this.Hide();
            var form2 = new GameForm();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Options();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new GameForm();
            var form3 = new GameForm();
            form2.Closed += (s, args) => this.Close();
            form3.Closed += (s, args) => this.Close();
            form2.Show();
            form3.Show();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            var users = new List<Joueur>();

            Joueur j = new Joueur("John");
            Joueur j1 = new Joueur("Lucy");
            Joueur j2 = new Joueur("Roger");

            j.wins = 10;

            Console.WriteLine(j.wins);
            users.Add(j);
            users.Add(j1);
            users.Add(j2);



            var writer = new StreamWriter("../../../Resources/users.csv");
            var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<Joueur>();
            csvWriter.NextRecord();
            csvWriter.WriteRecords(users);
            writer.Dispose();

            StreamReader streamReader = File.OpenText("../../../Resources/users.csv");
            CsvReader csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            users = csvReader.GetRecords<Joueur>().ToList();

            streamReader.Dispose();
            foreach (Joueur user in users)
            {
                Console.WriteLine(user.nom);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
