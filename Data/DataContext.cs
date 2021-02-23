using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSocial.Models;

namespace SSocial.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        }

        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Machine> Machines { get; set; }

        public DbSet<Log> Logs { get; set; }
        
        public DbSet<Record> Records { get; set; }
        
        public DbSet<SSocial.Models.Request> Request { get; set; }
        
        public DbSet<SSocial.Models.Repair> Repair { get; set; }
    }
}