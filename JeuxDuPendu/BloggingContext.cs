using Microsoft.EntityFrameworkCore;
using System;

namespace JeuxDuPendu
{
    public class BloggingContext : DbContext
    {
        //déclaration des objets à sauvegarder dans la base de données
        public DbSet<AsyncServer> servers { get; set; }

        public DbSet<AsyncClient> clients { get; set; }
        public DbSet<Joueur> joueurs { get; set; }

    public string DbPath { get; private set; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}game.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "Documents" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
