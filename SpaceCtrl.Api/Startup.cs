using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Api.Services;
using SpaceCtrl.Data.Database.DbObjects;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            services.Configure<AppSettings>(Configuration);

            ConfigureDatabase(services);

            services.AddScoped<DeviceService>();
            services.AddScoped<ObjectService>();
            services.AddSingleton<DeviceCache>();

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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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
            var swagger = new SwaggerOptions();
            Configuration.GetSection("Swagger").Bind(swagger);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swagger.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swagger.UiEndpoint, swagger.Description);
            });
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<SpaceCtrlContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}