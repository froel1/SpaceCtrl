using System;
using System.Collections.Generic;
using SpaceCtrl.Api.Models.Object;

namespace SpaceCtrl.Api.Models.Camera
{
    public class CameraObject
    {
        public List<Guid> Ids { get; set; } = default!;

        public CameraImage Image { get; set; } = default!;

        public Direction Direction { get; set; }
    }
}
