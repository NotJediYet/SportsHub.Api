using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SportsHub.Security;
using SportsHub.Shared.Models;
using SportsHub.Web.Validators;
using SportsHub.Shared.Entities;

namespace SportsHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationWithJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.Authority = configuration.GetValue<string>("Auth:Authority");
                   options.Audience = configuration.GetValue<string>("Auth:Audience");
               });

            return services;
        }

        public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                            .RequireAuthenticatedUser()
                                            .Build();

                options.AddPolicy(
                    Policies.User,
                    policy => policy.RequireClaim(Claims.Roles, Roles.User, Roles.Admin));

                options.AddPolicy(
                    Policies.Admin,
                    policy => policy.RequireClaim(Claims.Roles, Roles.Admin));
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme,
                                },
                            },
                            new List<string>()
                        },
                    });
            });

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateCategoryModel>, CreateCategoryModelValidator>();
            services.AddScoped<IValidator<CreateSubcategoryModel>, CreateSubcategoryModelValidator>();
            services.AddScoped<IValidator<CreateTeamModel>, CreateTeamModelValidator>();
            services.AddScoped<IValidator<Team>, EditTeamModelValidator>();
            services.AddScoped<IValidator<CreateArticleModel>, CreateArticleModelValidator>();

            return services;
        }
    }
}
