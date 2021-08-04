using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetBiere.Entity
{
    public class ProjetBiereContext : DbContext
    {

        public ProjetBiereContext(DbContextOptions<ProjetBiereContext> options) : base(options)
        {

        }

        //Entity
        public DbSet<Biere> Bieres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var styleConverter = new EnumToNumberConverter<Style, int>();

            modelBuilder
                .Entity<Biere>()
                .Property(e => e.Style)
                //.HasConversion(styleConverter);
                .HasConversion<int>();

            modelBuilder
                .Entity<Biere>()
                .HasData(new Biere()
                {
                    Id = 1,
                    Nom = "IPA 1",
                    ABV = 4.3,
                    IBU = 53,
                    Style = Style.IPA,
                    SRM = 4,
                    Saisonniere = 0
                }, new Biere()
                {
                    Id = 2,
                    Nom = "Blanche 1",
                    ABV = 5.2,
                    IBU = 20,
                    Style = Style.Blanche,
                    SRM = 5,
                    Saisonniere = 0
                }, new Biere()
                {
                    Id = 3,
                    Nom = "IPA 2",
                    ABV = 8,
                    IBU = 70,
                    Style = Style.IPA,
                    SRM = 2,
                    Saisonniere = 1
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
