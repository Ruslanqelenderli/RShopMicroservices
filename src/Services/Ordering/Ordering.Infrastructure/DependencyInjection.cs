
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInsfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");


            services.AddDbContext<ApplicationDbContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });



            return services;
        }
    }
}
