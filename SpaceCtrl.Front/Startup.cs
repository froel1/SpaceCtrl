using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Services;
using SpaceCtrl.Front.BackgroundServices;
using SpaceCtrl.Front.Models.Settings;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front
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

            services.AddScoped<ClientService>();
            services.AddScoped<RecordService>();
            services.AddScoped<SpaceCtrlContext>();
            services.AddScoped<GroupService>();
            services.AddSingleton<ISpaceCtrlCamera, SpaceCtrlCamera>();
            // services.AddHostedService<GroupSchedulerHostedService>();
            services.Configure<AppSettings>(Configuration);

            ConfigureDatabase(services);

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SpaceCtrl Front", Version = "v1" });
            });

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Configuration["Image:BasePath"]),
                RequestPath = Configuration["Image:StaticContentWebPath"]
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("DefaultPolicy");

            ConfigureSwagger(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Client}/{action=Index}/{id?}");
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