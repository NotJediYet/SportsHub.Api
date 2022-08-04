using Microsoft.Extensions.DependencyInjection;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Business.Services.Implementation;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<ITeamService, TeamService>();

            return services;
        }
    }
}
