using Microsoft.Extensions.DependencyInjection;

namespace SSocial.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:8080").AllowCredentials().AllowAnyHeader();
                });
            });
    }
}