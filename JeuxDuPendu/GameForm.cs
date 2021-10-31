using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using JeuxDuPendu.MyControls;

namespace JeuxDuPendu
{
    public partial class GameForm : Form
    {

        // Initialisation de l'instance de la classe d'affichage du pendu.
        HangmanViewer _HangmanViewer = new HangmanViewer();
        string line;
        int letterNumber;
        int essaisRestants;
        System.Media.SoundPlayer sp;

        /// <summary>
        /// Constructeur du formulaire de jeux
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            InitializeMyComponent();
            StartNewGame();
        }

        /// <summary>
        /// Initialisations des composant specifique a l'application
        /// </summary>
        private void InitializeMyComponent()
        {
            // On positionne le controle d'affichage du pendu dans panel1 : 
            panel1.Controls.Add(_HangmanViewer);
			
			// à la position 0,0
            _HangmanViewer.Location = new Point(0, 0);
			
			// et de la même taille que panel1
            _HangmanViewer.Size = panel1.Size;

            panel1.BackColor = Color.Transparent;
            panel1.Parent = pictureBox1;

            lCrypedWord.BackColor = Color.Transparent;
            lCrypedWord.Parent = pictureBox1;

            bReset.BackColor = Color.Transparent;
            bReset.Parent = pictureBox1;

            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            label2.BackColor = Color.Transparent;
            label2.Parent = pictureBox1;

            label3.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;

            /*richTextBox1.BackColor = Color.Transparent;
            richTextBox1.Parent = pictureBox1;*/
        }

        /// <summary>
        /// Initialise une nouvelle partie
        /// </summary>
        public void StartNewGame()
        {
            
            if (sp == null)
            {
                sp = new System.Media.SoundPlayer("../../../Resources/halloween-theme-song.wav");

                sp.PlayLooping();
            }
           

            var lines = File.ReadAllLines("../../../Resources/mots.txt");
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length - 1);
            line = lines[randomLineNumber];
            // Methode de reinitialisation classe d'affichage du pendu.
            _HangmanViewer.Reset();
            char motif = '_';
            letterNumber = line.Length;
            essaisRestants = 10;
            label2.Text = "Il vous reste " + letterNumber.ToString() + " lettre(s) !";
            label3.Text = "Il vous reste " + essaisRestants.ToString() + " essai(s) !";
            //listView1.Items.Clear();
            richTextBox1.Text = "";
            //Affichage du mot à trouver dans le label.


            lCrypedWord.Text = new String(motif, line.Length);
        }


        /// <summary>
        /// Methode appelé lors de l'appui d'un touche du clavier, lorsque le focus est sur le bouton "Nouvelle partie"
        /// </summary>
        private void bReset_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressed(e.KeyChar);
        }

        /// <summary>
        /// Methode appelé lors de l'appui d'un touche du clavier, lorsque le focus est sur le forulaire
        /// </summary>
        private void GameForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPressed(e.KeyChar);
        }

        /// <summary>
        /// Methode appelé lors de l'appui sur le bouton "Nouvelle partie"
        /// </summary>
        private void bReset_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        private bool HasSpecialChars(string yourString)
        {
            return yourString.Any(ch => !Char.IsLetter(ch));
                
        }

        private void KeyPressed(char letter)
        {
            //if (line.Contains(letter))
       
            if (!HasSpecialChars(letter.ToString()))
            {
                if (line.IndexOf(letter.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var sb = new StringBuilder(lCrypedWord.Text);
                    var before = sb.ToString();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i].ToString().IndexOf(letter.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            sb[i] = letter;
                        }
                    }
                    lCrypedWord.Text = sb.ToString();
                    if (!lCrypedWord.Text.Contains("_"))
                    {
                        MessageBox.Show("Vous avez gagné !");
                        StartNewGame();
                    }
                    //TODO corriger decrementation nombre de lettres
                    if (!before.Contains(letter))
                    {
                        int count = sb.ToString().Count(s => s == letter);
                        letterNumber -= count;
                    }

                    label2.Text = "Il vous reste " + letterNumber.ToString() + " lettre(s) !";
                }
                else
                {
                    if (!richTextBox1.Text.Contains(letter))
                    {
                        essaisRestants--;
                        _HangmanViewer.MoveNextStep();
                        richTextBox1.Text += letter + "\n";
                        label3.Text = "Il vous reste " + essaisRestants.ToString() + " essai(s) !";
                    }


                    //listView1.Items.Add(letter.ToString());
                    // On avance le pendu d'une etape


                    // Si le pendu est complet, le joueur à perdu.
                    if (_HangmanViewer.IsGameOver)
                    {
                        lCrypedWord.Text = line;
                        using (Form form = new Form())
                        {
                            Image img = Image.FromFile("../../../Resources/game_over.gif");

                            form.StartPosition = FormStartPosition.CenterScreen;
                            form.Size = img.Size;

                            PictureBox pb = new PictureBox();
                            pb.Dock = DockStyle.Fill;
                            pb.Image = img;

                            form.Controls.Add(pb);
                            sp = new System.Media.SoundPlayer("../../../Resources/sm64_bowser_laugh.wav");
                            sp.Play();
                            form.ShowDialog();
                        }
                        sp = null;
                        //MessageBox.Show("Vous avez perdu !");
                        StartNewGame();
                    }
                }
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();

            if (Application.OpenForms.Count > 2)
            {
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    Application.OpenForms[i].Close();
                }
                form2.Show();
            }
            else
            {
                this.Hide();
                var form3 = new Menu();
                form3.Closed += (s, args) => this.Close();
                form3.Show();
            }
            sp.Stop();

            /* this.Hide();
             var form2 = new Menu();
             Application.OpenForms.Cast<Form>().ToList().ForEach(x => form2.Closed += (s, args) => x.Close());
             form2.Show();*/

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

}
