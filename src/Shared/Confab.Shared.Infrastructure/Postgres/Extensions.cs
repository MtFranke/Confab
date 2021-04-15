using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Postgres
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgres(this IServiceCollection services)
        {
            var options = services.GetOptions<PostgresOptions>("postgres");
            services.AddSingleton(options);
            
            return services;
        }

        public static IServiceCollection AddPostgres<T>(this IServiceCollection services) where T : DbContext
        {
            PostgresOptions postgresOptions = services.GetOptions<PostgresOptions>("postgres");
            services.AddDbContext<T>(x => x.UseNpgsql(postgresOptions.ConnectionString));
            return services;
        }
    }
}