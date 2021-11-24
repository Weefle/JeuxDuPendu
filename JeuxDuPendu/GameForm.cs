using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JeuxDuPendu.MyControls;
using Microsoft.EntityFrameworkCore;

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

            //on gère ci-dessous l'affichage des éléments pour qu'ils soient transparents

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
            Debug.WriteLine("Mot: " + line);
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
            //fonction qui vérifie si on rentre bien une lettre et non autre chose
            return yourString.Any(ch => !Char.IsLetter(ch));
                
        }

        static string RemoveDiacritics(string text)
        {
            //fonction qui convertie les accents en lettres simples
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
            //on va effectuer ci-dessous le check sur chaque lettre
       
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
                    //si le mot ne contient plus "_", alors le joueur a gagné
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
                       
                        sp = null;
                        joueur.Wins++;

                        //on appelle la base de données pour supprimer le serveur ou le/les client(s)
                        if (server != null)
                        {
                            using (var db = new BloggingContext())
                            {
                                
                          

                                if (db.clients.Any(x => x.Name == joueur.Name))
                                    {

                                    db.clients.Where(x => x.Name == joueur.Name && x.Port == server.Port).First().Stop();
                                    db.clients.Remove(db.clients.AsNoTracking().Where(x => x.Name == joueur.Name).First());

                                    }
                                    if(db.servers.Any(x => x.Name == server.Name))
                                    {
                                        db.servers.AsNoTracking().Where(x => x.Name == server.Name).First().Stop();


                                        db.servers.Remove(server);


                                        db.clients.RemoveRange(db.clients);

                                    }
                                db.joueurs.Update(joueur);
                                db.SaveChanges();

                                
                            }
                            timer1.Enabled = false;

                        
                            this.Hide();
                            var form = new Menu();
                            form.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            using (var db = new BloggingContext())
                            {
                                db.joueurs.Update(joueur);
                                db.SaveChanges();
                            }
                                StartNewGame();
                        }
                    }
                    //on décrémente ici le nombre de lettres en fonction des lettres trouvées (accepte plusieurs lettres en même temps)
                    if (!before.Contains(letter))
                    {
                        int count = sb.ToString().Count(s => s == letter);
                        letterNumber -= count;
                    }

                    label2.Text = "Il vous reste " + letterNumber.ToString() + " lettre(s) !";
                }
                else
                {
                    //si les lettres fausses contiennent la lettre courante
                    if (!_wrongLetters.Contains(letter))
                    {
                        _wrongLetters.Add(letter);
                        essaisRestants--;
                        _HangmanViewer.MoveNextStep();
                        PictureBox pb = new PictureBox();
                        //on converti ici les lettres avec accents en lettres basiques pour utiliser les images
                        pb.Image = Image.FromFile("../../../Resources/letters/letter_" + RemoveDiacritics(letter.ToString()).ToUpperInvariant() + ".png");
                        pb.Location = new Point(100, pos);
                        pos+= 50;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Size = new Size(50, 50);
                        _wrongPictures.Add(pb);
                        this.Controls.Add(pb);
                        pb.BringToFront();
                     
                        label3.Text = "Il vous reste " + essaisRestants.ToString() + " essai(s) !";
                    }


               
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
                        joueur.Fails++;

                        if (server != null)
                        {
                            using (var db = new BloggingContext())
                            {


                                if (db.clients.Any(x => x.Name == joueur.Name && x.Port == server.Port))
                                {
                                    db.clients.Where(x => x.Name == joueur.Name && x.Port == server.Port).First().Stop();
                                    db.clients.Remove(db.clients.Where(x => x.Name == joueur.Name && x.Port == server.Port).First());

                                }
                                if (db.servers.Any(x => x.Name == server.Name))
                                {
                                    
                                    db.servers.AsNoTracking().Where(x => x.Name == server.Name).First().Stop();


                                    db.servers.Remove(server);


                                    db.clients.RemoveRange(db.clients.AsNoTracking().Where(x => x.Port == server.Port));

                                }


                                db.joueurs.Update(joueur);
                                db.SaveChanges();

                                
                            }
                            timer1.Enabled = false;

                       
                            this.Hide();
                            var form = new Menu();
                            form.ShowDialog();
                            this.Close();
                        }
                        else {
                            using (var db = new BloggingContext())
                            {
                                db.joueurs.Update(joueur);
                                db.SaveChanges();
                            }
                            StartNewGame();
                        }
                  
                       
                    }
                }
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            label1.Text = joueur.Name;
            label4.Visible = false;
            dataGridView1.Visible = false;
            //si le serveur existe (jeu en multi), alors on affiche la liste de joueurs
            if (server != null)
            {
                label4.Visible = true;
                label4.Text = server.Name;
                dataGridView1.Visible = true;
                timer1.Enabled = true;
            }
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
         //bouton pour quitter et supprimer le serveur ou le/les client(s)
            using (var db = new BloggingContext())
            {
                if (server != null)
                {

                    if (db.clients.Any(x => x.Name == joueur.Name && x.Port == server.Port))
                    {

                        db.clients.Where(x => x.Name == joueur.Name && x.Port == server.Port).First().Stop();
                        db.clients.Remove(db.clients.Where(x => x.Name == joueur.Name && x.Port == server.Port).First());

                    }
                    else
                    {
                        db.servers.AsNoTracking().Where(x => x.Name == server.Name).First().Stop();
                      

                        db.servers.Remove(server);
                      

                        db.clients.RemoveRange(db.clients.AsNoTracking().Where(x=>x.Port == server.Port));
                       
                    }

                    db.SaveChanges();

                }
            }
            timer1.Enabled = false;

            sp.Stop();
            this.Hide();
            var form = new Menu();
            form.ShowDialog();
            this.Close();

          

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //le timer permet de rafraichir la liste de joueurs
            using (var db = new BloggingContext())
            {
              
                    
                //si le serveur n'existe plus dans la base de données, on arrête le programme et les clients
                if(!db.servers.Contains(server))
                {
                    timer1.Enabled = false;
                    sp.Stop();
                    this.Hide();
                    var form = new Menu();
                    form.ShowDialog();
                    this.Close();
                }
                dataGridView1.DataSource = db.clients.AsNoTracking().Where(x => x.Port == server!.Port).ToList();

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

}
