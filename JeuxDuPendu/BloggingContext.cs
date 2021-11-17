using Microsoft.EntityFrameworkCore;
using System;

namespace JeuxDuPendu
{
    public class BloggingContext : DbContext
    {

        public DbSet<AsyncServer> servers { get; set; }

        public DbSet<Joueur> joueurs { get; set; }

    public string DbPath { get; private set; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}blogging.db";
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
