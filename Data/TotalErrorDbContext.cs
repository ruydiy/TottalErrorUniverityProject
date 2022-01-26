using Microsoft.EntityFrameworkCore;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class TotalErrorDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }

        public DbSet<ItemType> ItemTypes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<LastReadFile> LastReadFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-F8AKO5E9\MYSQLSERVER2017;Database=UniProjectTotalError;Trusted_Connection=True");
        }
        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries<IEntity<string>>();
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedAt = DateTime.Now;
                    entity.Entity.ModifiedAt = DateTime.Now;
                }
                else if (entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifiedAt = DateTime.Now;
                }
                else if (entity.State == EntityState.Detached)
                {
                    entity.State = EntityState.Deleted;
                    entity.Entity.DeletedAt = DateTime.Now;
                    entity.Entity.IsDeleted = true;
                }
            }

            return base.SaveChanges();
        }
    }
}
