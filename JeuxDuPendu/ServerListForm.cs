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
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }

        private void ServerListForm_Load(object sender, EventArgs e)
        {
            using (var db = new BloggingContext())
            {
                //dataGridView.DataSource = Program.servers;
                dataGridView1.DataSource = db.servers.ToList();
            }
        }

  

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AsyncServer server;
            AsyncClient client;
            using (var db = new BloggingContext())
            {
                server = db.servers.ToList()[e.RowIndex];


                if (!db.servers.Where(x => x.Name == server.Name).First().clients.Any(x => x.Name == joueur.Name))
                {
                    client = new AsyncClient(joueur.Name);
                    db.servers.Where(x => x.Name == server.Name).First().clients.Add(client);
                    db.SaveChanges();
                }
                else
                {
                    client = db.servers.Where(x => x.Name == server.Name).First().clients.Where(x => x.Name == joueur.Name).First();
                }

                
            }
            client.StartClient();

            this.Hide();
            var form2 = new GameForm(joueur, server);
            form2.ShowDialog();
            this.Close();

        }
    }
}
