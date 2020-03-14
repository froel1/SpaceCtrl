using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var settings = new AppSettings();
            Configuration.Bind(settings);
            services.AddSingleton(settings);
            ConfigureDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            var settings = new AppSettings();
            Configuration.Bind(settings);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = settings.Swagger.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(settings.Swagger.UiEndpoint, settings.Swagger.Description);
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<SpaceCtrlContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
