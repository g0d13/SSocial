using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Name = Role.Admin,
                    NormalizedName = Role.Admin.ToUpper()
                }, new Role
                {
                    Name = Role.Mechanic,
                    NormalizedName = Role.Mechanic.ToUpper()
                }, new Role
                {
                    Name = Role.Supervisor,
                    NormalizedName = Role.Supervisor.ToUpper()
                }
            );
        }
    }
}