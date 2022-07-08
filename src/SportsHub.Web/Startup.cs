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
        //Test
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBusiness();
            services.AddInfrastructure();

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
