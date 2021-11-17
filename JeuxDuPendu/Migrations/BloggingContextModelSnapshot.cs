﻿// <auto-generated />
using JeuxDuPendu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JeuxDuPendu.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("JeuxDuPendu.AsyncClient", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("AsyncServerName")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("AsyncServerName");

                    b.ToTable("AsyncClient");
                });

            modelBuilder.Entity("JeuxDuPendu.AsyncServer", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("servers");
                });

            modelBuilder.Entity("JeuxDuPendu.Joueur", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Fails")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Wins")
                        .HasColumnType("INTEGER");

                    b.HasKey("Name");

                    b.ToTable("joueurs");
                });

            modelBuilder.Entity("JeuxDuPendu.AsyncClient", b =>
                {
                    b.HasOne("JeuxDuPendu.AsyncServer", null)
                        .WithMany("clients")
                        .HasForeignKey("AsyncServerName");
                });

            modelBuilder.Entity("JeuxDuPendu.AsyncServer", b =>
                {
                    b.Navigation("clients");
                });
#pragma warning restore 612, 618
        }
    }
}
