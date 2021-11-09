using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Joueur j = new Joueur("John");
            Joueur j1 = new Joueur("Lucy");
            Joueur j2 = new Joueur("Roger");

            j.wins = 10;

           
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
