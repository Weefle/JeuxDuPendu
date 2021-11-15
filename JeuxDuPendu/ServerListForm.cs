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
        public ServerListForm()
        {
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
            CreateListViewItem(dataGridView1);
        }

        public static void CreateListViewItem(DataGridView dataGridView)
        {
            using (var db = new BloggingContext())
            {
                //dataGridView.DataSource = Program.servers;
                dataGridView.DataSource = db.servers.ToList();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            using (var db = new BloggingContext())
            {
                var selectedServer = db.servers.ToList()[e.RowIndex];
                AsyncClient client;
            }
           /* do
            {
                client = new AsyncClient(e.RowIndex);
            } while (Program.clients);
            
            client.StartClient();*/
        }
    }
}
