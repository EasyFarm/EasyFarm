using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;

namespace EasyFarm.DataContexts
{
    public class EasyFarmDB : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public EasyFarmDB()
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseAlways<EasyFarmDB>(
                Database.Connection.ConnectionString, modelBuilder);

            Database.SetInitializer(sqliteConnectionInitializer);

            modelBuilder.Entity<Action>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Magic>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Health>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Ignored>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Targeted>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Battle>().HasKey(x => x.Id)
                .HasRequired(x => x.Player)
                .WithRequiredDependent();
            modelBuilder.Entity<Player>()
                .HasKey(x => x.Id);
        }
    }
}
