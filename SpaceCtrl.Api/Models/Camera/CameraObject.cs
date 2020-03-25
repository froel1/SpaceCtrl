using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceCtrl.Api.Models.Camera
{
    public class CameraObject
    {
        public Direction Direction { get; set; }
        public string Date { get; set; } = default!;

        public Dictionary<string, DataModel> Data { get; set; } = default!;
    }

    public class DataModel
    {
        public string Base64Image { get; set; } = default!;

        public string ImageType { get; set; } = default!;
        public string Date { get; set; } = default!;

        public int PersonCount { get; set; }
    }
}