using Microsoft.Extensions.DependencyInjection;
using SportsHub.Business.Services;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubcategoryService, SubcategoryService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ILogoService, LogoService>();

            return services;
        }
    }
}
