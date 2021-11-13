using CsvHelper;
using Microsoft.VisualBasic;
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

            button5.BackColor = Color.Transparent;
            button5.Parent = pictureBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*this.Hide();
            var form2 = new GameForm();
            form2.Closed += (s, args) => this.Close();
            form2.Show();*/
            Joueur joueur;

            const string mess =
 "Quel est votre nom ?";
            var reslt = Interaction.InputBox(mess, "Nom du joueur");

            // If the no button was pressed ...
            if (!string.IsNullOrEmpty(reslt))
            {

                if (!Program.joueurs.Any(x => x.Name == reslt))
                {
                    joueur = new Joueur(reslt, 0, 0);
                    Program.joueurs.Add(joueur);
                }

                else
                {
                    joueur = Program.joueurs.Where(x => x.Name == reslt).First();
                }

                this.Hide();
                var form2 = new GameForm(joueur, null);
                //form2.Closed += (s, args) => this.Close();
                form2.ShowDialog();
                this.Close();

            }
            
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

            Joueur joueur;

            const string mess =
 "Quel est votre nom ?";
            var reslt = Interaction.InputBox(mess, "Nom du joueur");

            // If the no button was pressed ...
            if (!string.IsNullOrEmpty(reslt))
            {
               
                if (!Program.joueurs.Any(x => x.Name == reslt))
                {
                    joueur = new Joueur(reslt, 0, 0);
                    Program.joueurs.Add(joueur);
                }

                else
                {
                    joueur = Program.joueurs.Where(x => x.Name == reslt).First();
                }



                const string msg =
                "Voulez vous créer un serveur ?";
                    const string caption = "Multijoueur";
                    var result = MessageBox.Show(msg, caption,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);


                    if (result == DialogResult.Yes)
                    {
                        const string message =
         "Donnez un nom à votre serveur";
                        var res = Interaction.InputBox(message, "Création du serveur");
                    AsyncServer server;
                        // If the no button was pressed ...
                        if (!string.IsNullOrEmpty(res))
                        {
                        if (!Program.servers.Any(x => x.Name == res))
                        {
                            server = new AsyncServer(res);
                            Program.servers.Add(server);
                        }

                        else
                        {
                            server = Program.servers.Where(x => x.Name == res).First();
                        }
                        
                            server.StartServer();
                            this.Hide();
                            var form2 = new GameForm(joueur, server);
                            /*var form3 = new GameForm();
                            form2.Closed += (s, args) => this.Close();
                            form3.Closed += (s, args) => this.Close();
                            form2.Show();*/
                            form2.ShowDialog();
                            this.Close();
                        }
                    }
                    else if (result == DialogResult.No)
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


            }
           
    }

        private void Menu_Load(object sender, EventArgs e)
        {
           
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

        private void button5_Click(object sender, EventArgs e)
        {
            //Program.RefreshData();
            this.Close();
        }
    }
}
