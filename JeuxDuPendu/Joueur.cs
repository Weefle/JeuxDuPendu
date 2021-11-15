using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    
    public class Joueur
    {
        [Key] public string Name { get; set; }
        public int Wins { get; set; }
        public int Fails { get; set; }

        public Joueur(string Name, int Wins, int Fails)
        {
            this.Name = Name;
            this.Wins = Wins;
            this.Fails = Fails;
        }



    }
}
