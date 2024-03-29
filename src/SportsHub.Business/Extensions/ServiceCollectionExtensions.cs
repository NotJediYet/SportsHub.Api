﻿using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ILanguageService, LanguageService>();

            return services;
        }
    }
}