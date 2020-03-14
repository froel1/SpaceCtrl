using System;
using System.Collections.Generic;

namespace SpaceCtrl.Api.Models.Camera
{
    public class CameraObject
    {
        public Direction Direction { get; set; }
        public string Date { get; set; } = default!;
        public Guid DeviceKey { get; set; }
        public List<Guid> Objects { get; set; } = default!;
        public CameraImage Image { get; set; }
    }
}
