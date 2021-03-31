using System;
using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext:  IdentityDbContext<User, Role, Guid>
    {
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
        
    public DbSet<Machine> Machines { get; set; }
        
    public DbSet<Category> Categories { get; set; }
    public DbSet<Log> Logs { get; set; }
        
    public DbSet<Record> Records { get; set; }
        
    public DbSet<Request> Request { get; set; }
        
    public DbSet<Repair> Repair { get; set; }
    }
}