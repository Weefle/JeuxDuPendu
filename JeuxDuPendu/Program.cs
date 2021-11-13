using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuxDuPendu
{
    static class Program
    {

        public static List<Joueur> joueurs = new List<Joueur>();
        public static string fichierMots { get; set; } = "../../../Resources/mots.txt";

        public static string fichierSauvegarde { get; set; } = "../../../Resources/users.csv";

        public static string fichierServers { get; set; } = "../../../Resources/servers.csv";

        public static string fichierClients { get; set; } = "../../../Resources/clients.csv";

        public static List<AsyncServer> servers = new List<AsyncServer>();

        public static List<AsyncClient> clients = new List<AsyncClient>();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var thread = new Thread(RefreshData) { IsBackground = true };
            thread.Start();


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }

        public static void RefreshData()
        {
            while (true)
            {
                using (var reader = new StreamReader(fichierSauvegarde))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    joueurs = csv.GetRecords<Joueur>().ToList();
                    csv.Dispose();
                }

                using (var reader = new StreamReader(fichierServers))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    servers = csv.GetRecords<AsyncServer>().ToList();
                    csv.Dispose();
                }

                /*using (var reader = new StreamReader(fichierClients))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    clients = csv.GetRecords<AsyncClient>().ToList();
                    csv.Dispose();
                }*/

                Thread.Sleep(1000);

                using (var writer = new StreamWriter(Program.fichierSauvegarde))
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    csv.WriteRecords(Program.joueurs);
                    csv.Dispose();
                }

                using (var writer = new StreamWriter(Program.fichierServers))
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    csv.WriteRecords(Program.servers);
                    csv.Dispose();
                }

                /*using (var writer = new StreamWriter(Program.fichierClients))
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    csv.WriteRecords(Program.clients);
                    csv.Dispose();
                }*/
            }
        }

    }
}
