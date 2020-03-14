using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            ConfigureDatabase(services);

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SpaceCtrl Front", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            ConfigureSwagger(app);

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
