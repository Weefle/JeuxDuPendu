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
    public partial class Scores : Form
    {
        public Scores()
        {
            InitializeComponent();
            //on gère ci-dessous l'affichage des éléments pour qu'ils soient transparents
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

         
        }

        private void Scores_Load(object sender, EventArgs e)
        {
            

        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            //retour au menu
            this.Hide();
            var form2 = new Menu();
           
            form2.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //rafraichissement de la liste des joueurs
            using (var db = new BloggingContext())
            {
                dataGridView1.DataSource = db.joueurs.ToList<Joueur>();
            }
        }
    }
}
