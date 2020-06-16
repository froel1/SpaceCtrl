using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceCtrl.Data.Extensions;

namespace SpaceCtrl.Api.Models.Settings
{
    public class ImageSettings : IImageSettings
    {
        public string BasePath { get; set; } = default!;
        public string UserImagesPath { get; set; } = default!;
        public string StaticContentWebPath { get; set; } = string.Empty;
    }
}