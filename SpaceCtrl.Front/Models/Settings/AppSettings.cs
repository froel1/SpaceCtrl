namespace SpaceCtrl.Front.Models.Settings
{
    public class AppSettings
    {
        public SwaggerSettings Swagger { get; set; } = default!;

        public ImageSettings Image { get; set; } = default!;
    }
}
