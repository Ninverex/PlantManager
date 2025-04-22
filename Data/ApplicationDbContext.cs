using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MenadzerRoslin.Models;
using System;
using System.IO;

namespace MenadzerRoslin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Roslina> Rosliny { get; set; }
        public DbSet<Gatunek> Gatunki { get; set; }
        public DbSet<Zabieg> Zabiegi { get; set; }
        public DbSet<Przypomnienie> Przypomnienia { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Bezpośrednie wczytanie konfiguracji dla migracji
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja relacji
            modelBuilder.Entity<Roslina>()
                .HasOne(r => r.Gatunek)
                .WithMany(g => g.Rosliny)
                .HasForeignKey(r => r.GatunekId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zabieg>()
                .HasOne(z => z.Roslina)
                .WithMany(r => r.Zabiegi)
                .HasForeignKey(z => z.RoslinaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Przypomnienie>()
                .HasOne(p => p.Roslina)
                .WithMany(r => r.Przypomnienia)
                .HasForeignKey(p => p.RoslinaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja wszystkich właściwości DateTime, aby używały 'timestamp without time zone'
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp without time zone");
                    }
                }
            }
            
        }
    }
}