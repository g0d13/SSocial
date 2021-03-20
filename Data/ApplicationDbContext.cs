using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSocial.Models;

namespace SSocial.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().Ignore(c => c.Role);
        }
        
        public DbSet<Machine> Machines { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Log> Logs { get; set; }
        
        public DbSet<Record> Records { get; set; }
        
        public DbSet<Request> Request { get; set; }
        
        public DbSet<Repair> Repair { get; set; }
    }
}