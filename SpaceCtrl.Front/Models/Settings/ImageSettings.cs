using System.Collections.Generic;

namespace SpaceCtrl.Front.Models.Settings
{
    public class ImageSettings
    {
        public string BasePath { get; set; } = default!;

        public List<string> AllowedExtensions { get; set; } = default!;

        public int MaxSize { get; set; }
    }
}