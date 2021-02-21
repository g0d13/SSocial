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
        
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Machine> Machines { get; set; }

        public DbSet<Log> Logs { get; set; }
        
        public DbSet<Record> Records { get; set; }
    }
}