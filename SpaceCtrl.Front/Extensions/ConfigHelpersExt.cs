using Microsoft.AspNetCore.Builder;

namespace SpaceCtrl.Front.Extensions
{
    public static class ConfigHelpersExt
    {
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "Face Ctrl Api");
            });

            return app;
        }
    }
}
