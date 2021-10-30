using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    internal class Joueur
    {
        public string nom { get; set; }
        public int wins { get; set; }
        public int fails { get; set; }


        public Joueur(string nom)
        {
            this.nom = nom;
            this.wins = 0;
            this.fails = 0;
        }

    }
}
