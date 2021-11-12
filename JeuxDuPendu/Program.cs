using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuxDuPendu
{
    static class Program
    {

        public static List<Joueur> joueurs = new List<Joueur>();
        public static string fichierMots { get; set; } = "../../../Resources/mots.txt";

        public static string fichierSauvegarde { get; set; } = "../../../Resources/users.csv";

        public static List<AsyncServer> servers = new List<AsyncServer>();

        public static List<AsyncClient> clients = new List<AsyncClient>();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Joueur j = new Joueur("John", 10, 5);
            Joueur j1 = new Joueur("Lucy", 6, 2);
            Joueur j2 = new Joueur("Bob", 4, 10);


            joueurs.Add(j);
            joueurs.Add(j1);
            joueurs.Add(j2);



            using (var writer = new StreamWriter(fichierSauvegarde))
            using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.WriteRecords(joueurs);
               
            }

            using (var reader = new StreamReader(fichierSauvegarde))
            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
            {
                joueurs = csv.GetRecords<Joueur>().ToList();
                
            }

            

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
        }
    }
}
