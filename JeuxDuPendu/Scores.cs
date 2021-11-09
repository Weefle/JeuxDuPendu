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
        }

        private void Scores_Load(object sender, EventArgs e)
        {
            foreach(Joueur joueur in Program.joueurs)
            {
                CreateListViewItem(listView1, joueur);
            }
        }

        public static void CreateListViewItem(ListView listView, Object obj)
        {
            ListViewItem item = new ListViewItem();
            item.Text = ((Joueur)obj).nom;
            item.Tag = (Joueur)obj;
            item.SubItems.Add(((Joueur)obj).wins.ToString());
            item.SubItems.Add(((Joueur)obj).fails.ToString());

            // Other requirements as needed

            listView.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Menu();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
    }
}
