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
     
            dataGridView.DataSource = Program.servers;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedServer = Program.servers[e.RowIndex];
            AsyncClient client;
           /* do
            {
                client = new AsyncClient(e.RowIndex);
            } while (Program.clients);
            
            client.StartClient();*/
        }
    }
}
