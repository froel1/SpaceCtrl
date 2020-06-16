using System.Collections.Generic;
using SpaceCtrl.Data.Extensions;

namespace SpaceCtrl.Front.Models.Settings
{
    public class ImageSettings : IImageSettings
    {
        public string BasePath { get; set; } = default!;
        public string UserImagesPath { get; set; } = default!;
        public string StaticContentWebPath { get; set; } = default!;

        public List<string> AllowedExtensions { get; set; } = default!;

        public int MaxSize { get; set; }
    }
}