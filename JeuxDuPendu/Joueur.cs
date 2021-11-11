using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace JeuxDuPendu
{
    public class Joueur
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Fails { get; set; }


        public class BalgenModelClassMap : ClassMap<Joueur>
        {
            /// <summary>
            ///     Constructeur permettant de mapper les champs d'un fichier CSV avec le mod�le BalgenModel
            /// </summary>
            public BalgenModelClassMap()
            {
                Map(m => m.Name).Name("Nom");
                Map(m => m.Wins).Name("Wins");
                Map(m => m.Fails).Name("Fails");
          
            }
        }

    }
}
