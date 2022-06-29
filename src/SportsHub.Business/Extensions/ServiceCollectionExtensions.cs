using Microsoft.Extensions.DependencyInjection;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            return services;
        }
    }
}
