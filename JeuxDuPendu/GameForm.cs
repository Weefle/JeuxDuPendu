using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        List<char> _wrongLetters = new List<char>();
        List<PictureBox> _wrongPictures = new List<PictureBox>();
        string line;
        int letterNumber;
        int essaisRestants;
        int pos = 0;
        System.Media.SoundPlayer sp;
        Joueur joueur;
        AsyncServer? server;

        /// <summary>
        /// Constructeur du formulaire de jeux
        /// </summary>
        public GameForm(Joueur joueur, AsyncServer? server)
        {
            this.joueur = joueur;
            this.server = server;
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

            label1.BackColor = Color.Transparent;
            label1.Parent = pictureBox1;

            label4.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;

            /*richTextBox1.BackColor = Color.Transparent;
            richTextBox1.Parent = pictureBox1;*/
        }

        /// <summary>
        /// Initialise une nouvelle partie
        /// </summary>
        public void StartNewGame()
        {
            using (var db = new BloggingContext())
            {

                db.joueurs.Update(joueur);
                db.SaveChanges();

            }
            foreach (PictureBox item in _wrongPictures)
            {

                this.Controls.Remove(item);


            }
            _wrongPictures.Clear();
            _wrongLetters.Clear();
            pos = 0;
            
            if (sp == null)
            {
                sp = new System.Media.SoundPlayer("../../../Resources/halloween-theme-song.wav");

                sp.PlayLooping();
            }
           

            var lines = File.ReadAllLines(Program.fichierMots);
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
            //richTextBox1.Text = "";
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

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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
                        using (Form form = new Form())
                        {
                            Image img = Image.FromFile("../../../Resources/mario_stage_end.gif");

                            form.StartPosition = FormStartPosition.CenterScreen;
                            form.Size = img.Size;

                            PictureBox pb = new PictureBox();
                            pb.Dock = DockStyle.Fill;
                            pb.Image = img;

                            form.Controls.Add(pb);
                            sp = new System.Media.SoundPlayer("../../../Resources/smb_stage_clear.wav");
                            sp.Play();
                            form.ShowDialog();
                        }
                        joueur.Wins++;
                       
                        sp = null;


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
                    //if (!richTextBox1.Text.Contains(letter))
                    if (!_wrongLetters.Contains(letter))
                    {
                        _wrongLetters.Add(letter);
                        essaisRestants--;
                        _HangmanViewer.MoveNextStep();
                        PictureBox pb = new PictureBox();
                        //TODO corriger problème avec les accents !
                        pb.Image = Image.FromFile("../../../Resources/letters/letter_" + RemoveDiacritics(letter.ToString()).ToUpperInvariant() + ".png");
                        pb.Location = new Point(100, pos);
                        pos+= 50;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Size = new Size(50, 50);
                        _wrongPictures.Add(pb);
                        this.Controls.Add(pb);
                        pb.BringToFront();
                        //richTextBox1.Text += letter + "\n";
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
                        joueur.Fails++;
                   
                        sp = null;
                       
                        
                        
                        //MessageBox.Show("Vous avez perdu !");
                        StartNewGame();
                    }
                }
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            label1.Text = joueur.Name;
            label4.Visible = false;
            if (server != null)
            {
                label4.Visible = true;
                label4.Text = server.Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Program.RefreshData();
            using (var db = new BloggingContext())
            {
                if (server != null)
                {
                    db.servers.Remove(server);
                    db.SaveChanges();
                    //db.servers.Remove(db.servers.Where(x => x.Name == server.Name).FirstOrDefault());
                }
            }
             
            sp.Stop();
            this.Hide();
            var form = new Menu();
            form.ShowDialog();
            this.Close();

            /*if (Application.OpenForms.Count > 1)
            {
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    Application.OpenForms[i].Close();
                }
                var form2 = new Menu();
                form2.Show();
            }
            else
            {
          
                var form3 = new Menu();
                form3.Closed += (s, args) => this.Close();
                form3.Show();
            }*/
           

            /* this.Hide();
             var form2 = new Menu();
             Application.OpenForms.Cast<Form>().ToList().ForEach(x => form2.Closed += (s, args) => x.Close());
             form2.Show();*/

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
