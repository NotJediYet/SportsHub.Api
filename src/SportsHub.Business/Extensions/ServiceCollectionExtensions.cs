using Microsoft.Extensions.DependencyInjection;
using SportsHub.Business.Services;
using Microsoft.AspNetCore.Http;

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

        public static byte[] ByteArray(this IFormFile fileLogo)
        {
            using var memoryStream = new MemoryStream();
            fileLogo.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
