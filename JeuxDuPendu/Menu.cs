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
using Button = System.Windows.Forms.Button;
using TextBox = System.Windows.Forms.TextBox;

namespace JeuxDuPendu
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            //initialisation des éléments avec transparence
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

            //solo
            Joueur joueur;

            const string mess =
 "Quel est votre nom ?";
            var reslt = Interaction.InputBox(mess, "Nom du joueur");

            // If the no button was pressed ...
            if (!string.IsNullOrEmpty(reslt))
            {
                using (var db = new BloggingContext())
                {
                    if (!db.joueurs.Any(x => x.Name == reslt))
                    {
                        joueur = new Joueur(reslt, 0, 0);
                        db.joueurs.Add(joueur);
                        db.SaveChanges();
                    }

                    else
                    {
                        joueur = db.joueurs.Where(x => x.Name == reslt).First();
                    }

                    this.Hide();
                    var form2 = new GameForm(joueur, null);
         
                    form2.ShowDialog();
                    this.Close();
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //options
            this.Hide();
            var form2 = new Options();
           
            form2.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //multi
            Joueur joueur;

            const string mess =
 "Quel est votre nom ?";
            var reslt = Interaction.InputBox(mess, "Nom du joueur");

            // If the no button was pressed ...
            if (!string.IsNullOrEmpty(reslt))
            {
                using (var db = new BloggingContext())
                {

                    if (!db.joueurs.Any(x => x.Name == reslt))
                {
                    joueur = new Joueur(reslt, 0, 0);
                        db.joueurs.Add(joueur);
                        db.SaveChanges();
                    }

                else
                {
                    joueur = db.joueurs.Where(x => x.Name == reslt).First();
                }



                const string msg =
                "Voulez vous créer un serveur ou rejoindre un serveur ?";
                const string caption = "Multijoueur";
                    /*var result = MessageBox.Show(msg, caption,
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);*/
                    var result = DialogBox(caption, msg, null, "Créer", "Rejoindre", null);


                if (result == DialogResult.Yes)
                {
                    const string message =
     "Donnez un nom à votre serveur";
                    var res = Interaction.InputBox(message, "Création du serveur");
                    AsyncServer server;
                    // If the no button was pressed ...
                    if (!string.IsNullOrEmpty(res))
                    {
                        using (var db1 = new BloggingContext())
                        {
                                
                             if (!db1.servers.Any(x => x.Name == res))
                         {
                             server = new AsyncServer(res);
                                    db1.servers.Add(server);
                                    db1.SaveChanges();
                                }

                         else
                         {
                             server = db1.servers.Where(x => x.Name == res).First();
                         }

                         
                        }
                            server.StartServer();
                            this.Hide();
                            var form2 = new GameForm(joueur, server);

                            form2.ShowDialog();
                            this.Close();
                        }
                }
                else if (result == DialogResult.No)
                {
                    this.Hide();
                    var form2 = new ServerListForm(joueur);
       
                    form2.ShowDialog();
                    this.Close();
                }

            }
            }
           
    }

        public static DialogResult DialogBox(string title, string promptText, string value, string button1 = "Yes", string button2 = "No", string button3 = "Cancel")
        {
            //cette fonction permet l'affichage d'une MessageBox personnalisée
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button button_1 = new Button();
            Button button_2 = new Button();
            Button button_3 = new Button();

            int buttonStartPos = 228; //Standard two button position


            if (button3 != null)
                buttonStartPos = 228 - 85;
            else
            {
                button_3.Visible = false;
                button_3.Enabled = false;
            }


            form.Text = title;

            // Label
            label.Text = promptText;
            label.SetBounds(9, 20, 372, 13);
            label.Font = new Font("Microsoft Tai Le", 10, FontStyle.Regular);

            // TextBox
            if (value == null)
            {
            }
            else
            {
                textBox.Text = value;
                textBox.SetBounds(12, 36, 372, 20);
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            }

            button_1.Text = button1;
            button_2.Text = button2;
            button_3.Text = button3 ?? string.Empty;
            button_1.DialogResult = DialogResult.Yes;
            button_2.DialogResult = DialogResult.No;
            button_3.DialogResult = DialogResult.Cancel;


            button_1.SetBounds(buttonStartPos, 75, 80, 40);
            button_2.SetBounds(buttonStartPos + 85, 75, 85, 40);
            button_3.SetBounds(buttonStartPos + (2 * 85), 75, 80, 40);

            label.AutoSize = true;
            button_1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(400, 150);
            form.Controls.AddRange(new Control[] { label, button_1, button_2 });
            if (button3 != null)
                form.Controls.Add(button_3);
            if (value != null)
                form.Controls.Add(textBox);

            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = button_1;
            form.CancelButton = button_2;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //scores
            this.Hide();
            var form2 = new Scores();
          
            form2.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           //quitte le programme
            this.Close();
        }
    }
}
