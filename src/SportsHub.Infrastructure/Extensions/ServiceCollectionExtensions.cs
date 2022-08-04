using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsHub.Infrastructure.DBContext;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration сonfiguration)
        {
            services.AddDbContext<SportsHubDbContext>(options =>
                options.UseSqlServer(сonfiguration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
