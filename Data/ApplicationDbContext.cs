using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SSocial.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Ignore(c => c.Role);

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = "5246695f-aa58-4ddc-9c2b-1e9458a2c223",
                Email = "admin@admin.com",
                PasswordHash = hasher.HashPassword(null, "admin"),
                UserName = "Admin",
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "2894ae86-3c7b-4429-8d4d-db637636aa00",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole {Name = "Supervisor", NormalizedName = "SUPERVISOR"},
                new IdentityRole {Name = "Mechanic", NormalizedName = "MECHANIC"}

            );
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(
                    new IdentityUserRole<string>
                    {
                        RoleId = "2894ae86-3c7b-4429-8d4d-db637636aa00",
                        UserId = "5246695f-aa58-4ddc-9c2b-1e9458a2c223"
                    }
                );
        }
    }
}