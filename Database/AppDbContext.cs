

using RestApiNet5.Data.Models;
using RestApiNet5.Database.Seeders;
using RestApiNet5.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RestApiNet5.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connection = "FileName=db.sqlite";
            optionsBuilder.UseLazyLoadingProxies().UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RunDatabaseSeeder();
        }

        public override int SaveChanges()
        {
            ChangeTracker.Entries<ModelBase>().ToList().ForEach(x=>x.Entity.TrackAndUpdateTimestamps());
            return base.SaveChanges();
        }

    }

     public static class DbSetExtensions
    {
        public static T FindOrFail<T>(this DbSet<T> dbSet, string id) where T : class {
            return dbSet.Find(id) ?? throw new RecordNotFoundException();
        }
    }
}