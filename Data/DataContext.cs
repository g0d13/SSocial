using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSocial.Configuration;
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
            modelBuilder.Entity<RefreshToken>()
                .ToTable("RefreshToken", t => t.ExcludeFromMigrations());
        }
        
        public DbSet<Machine> Machines { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Log> Logs { get; set; }
        
        public DbSet<Record> Records { get; set; }
        
        public DbSet<Request> Request { get; set; }
        
        public DbSet<Repair> Repair { get; set; }
    }
}