using System.Collections.Generic;

namespace SpaceCtrl.Data.Services
{
    public class SpaceCtrlCameraSettings
    {
        public string ServerAddress { get; set; } = default!;

        public List<IpCamera> IpCameras { get; set; } = default!;
    }
}