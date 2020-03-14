using System.Collections.Generic;

namespace SpaceCtrl.Data.Services
{
    public class FaceCtrlCameraSettings
    {
        public string ServerAddress { get; set; } = default!;

        public List<IpCamera> IpCameras { get; set; } = default!;
    }
}