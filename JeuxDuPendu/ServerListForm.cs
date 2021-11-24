using System;
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

        public Joueur joueur;
        public ServerListForm(Joueur joueur)
        {
            this.joueur = joueur;
            InitializeComponent();
            //on gère ci-dessous l'affichage des éléments pour qu'ils soient transparents
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //retour au menu
            this.Hide();
            var form2 = new Menu();
          
            form2.ShowDialog();
            this.Close();
        }

        private void ServerListForm_Load(object sender, EventArgs e)
        {
        
        }

  

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //évènement lors du clic sur un serveur
            AsyncServer server;
            AsyncClient client;
            using (var db = new BloggingContext())
            {
                server = db.servers.ToList()[e.RowIndex];


                if (!db.clients.Any(x => x.Name == joueur.Name))
                {
                    client = new AsyncClient(joueur.Name, server.Port);
                    db.clients.Add(client);
                    db.SaveChanges();
                }
                else
                {
                    client = db.clients.Where(x => x.Name == joueur.Name).First();
                }

                
            }
            client.StartClient();

            this.Hide();
            var form2 = new GameForm(joueur, server);
            form2.ShowDialog();
            this.Close();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //rafraichissement de la liste des serveurs
            using (var db = new BloggingContext())
            {

                dataGridView1.DataSource = db.servers.ToList();

            

            }
        }
    }
}
