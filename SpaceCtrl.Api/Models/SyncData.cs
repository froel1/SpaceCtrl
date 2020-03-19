using System;
using System.Collections.Generic;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Data.Models.ClientSync;

namespace SpaceCtrl.Api.Models
{
    public class SyncData
    {
        public Guid ClientId { get; set; }
        public List<CameraImage>? Images { get; set; }

        public SyncOperationType Type { get; set; }

        public SyncData(Guid clientId, List<CameraImage>? images, SyncOperationType type)
        {
            ClientId = clientId;
            Images = images;
            Type = type;
        }
    }
}