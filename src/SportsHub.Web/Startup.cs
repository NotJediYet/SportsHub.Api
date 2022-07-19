using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Okta.AspNetCore;
using SportsHub.Extensions;

namespace SportsHub.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusiness();
            services.AddInfrastructure();

            services.AddAuthentication(OktaDefaults.ApiAuthenticationScheme)
                .AddOktaWebApi(new OktaWebApiOptions()
                {
                    OktaDomain = Configuration["Okta:Domain"],
                    AuthorizationServerId = Configuration["Okta:AuthorizationServerId"]
                });


            services.AddAuthorization();

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen(option =>
                {
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id=JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                });
                services.AddCors();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(swagger =>
                {
                    swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "SportsHub API V1");
                });

                app.UseCors(builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
