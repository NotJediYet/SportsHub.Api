using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportsHub.Infrastructure.DBContext;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connection)
        {
            services.AddDbContext<SportsHubDbContext>(options =>
            options.UseSqlServer(connection));
            return services;
        }
    }
}
