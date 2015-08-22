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
        public DbSet<Action> Action { get; set; }
        public DbSet<Battle> Battle { get; set; }
        public DbSet<Health> Health { get; set; }
        public DbSet<Ignored> Ignored { get; set; }
        public DbSet<Magic> Magic { get; set; }
        public DbSet<Targeted> Targeted { get; set; }

        public EasyFarmDB()
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<EasyFarmDB>(
                Database.Connection.ConnectionString, modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
