using Campeonato.Dominio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campeonato.RepositorioEF
{
    public class Contexto : DbContext
    {
        public Contexto()
            : base("CampeonatoConfig")
        {

        }

        public DbSet<Time> Times { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Time>().Property(x => x.Nome).IsRequired().HasColumnType("varchar").HasMaxLength(75);
            modelBuilder.Entity<Time>().Property(x => x.DataFundacao).IsRequired().HasColumnType("date");
        }
    }
}
