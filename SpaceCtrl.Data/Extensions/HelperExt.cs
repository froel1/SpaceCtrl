using System.IO;
using SpaceCtrl.Data.Database.DbObjects;

namespace SpaceCtrl.Data.Extensions
{
    public static class HelperExt
    {
        public static string GetPath(this Frame frame, IImageSettings settings, ImageType type)
        {
            return type switch
            {
                ImageType.User => Path.Combine(settings.BasePath, settings.UserImagesPath, $"{frame.Id}{frame.Type}"),
                ImageType.Object => Path.Combine(settings.BasePath, $"{frame.Id}{frame.Type}"),
                _ => settings.BasePath
            };
        }

        public static string GetWebPath(this Frame frame, IImageSettings settings, ImageType type)
        {
            return type switch
            {
                ImageType.User => Path.Combine(settings.StaticContentWebPath, settings.UserImagesPath, $"{frame.Id}{frame.Type}"),
                ImageType.Object => Path.Combine(settings.StaticContentWebPath, $"{frame.Id}{frame.Type}"),
                _ => settings.BasePath
            };
        }
    }

    public interface IImageSettings
    {
        string BasePath { get; set; }
        string UserImagesPath { get; set; }
        string StaticContentWebPath { get; set; }
    }

    public enum ImageType
    {
        Object = 1,
        User = 2
    }
}