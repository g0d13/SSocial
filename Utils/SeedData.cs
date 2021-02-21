using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSocial.Data;

namespace SSocial.Utils
{
    public static class MigrateData
    {
        public static IHost InitializeDatabase(this IHost host)
        {
            CreateData(host);

            return host;
        }

        private static async void CreateData(IHost host)
        {
            var scope = host.Services.CreateScope();
            
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            await appContext.Database.EnsureCreatedAsync();

            const string userName = "Admin";
            const string userPwd = "Admin1";
            
            var user = await userManager.FindByNameAsync(userName);

            // If admin was created omit
            if (user != null) return;

            user = new ApplicationUser()
            {
                UserName = userName,
                EmailConfirmed = true,
            };
            
            await userManager.CreateAsync(user, userPwd);
            
            await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            await roleManager.CreateAsync(new IdentityRole(Role.Mechanic));
            await roleManager.CreateAsync(new IdentityRole(Role.Supervisor));

            await userManager.AddToRoleAsync(user, Role.Admin);
        }
    }
}