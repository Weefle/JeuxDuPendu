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
            button1.BackColor = Color.Transparent;
            button1.Parent = pictureBox1;

         
        }

        private void Scores_Load(object sender, EventArgs e)
        {
           
                CreateListViewItem(dataGridView1);
            
        }

        public static void CreateListViewItem(DataGridView dataGridView)
        {
            using (var db = new BloggingContext())
            {
                dataGridView.DataSource = db.joueurs.ToList<Joueur>();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();
            //form2.Closed += (s, args) => this.Close();
            form2.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
