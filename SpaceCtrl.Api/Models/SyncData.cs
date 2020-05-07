using System;
using System.Collections.Generic;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Data.Models.ClientSync;

namespace SpaceCtrl.Api.Models
{
    public class SyncData
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public List<CameraImage>? Images { get; set; }

        public SyncOperationType Type { get; set; }

        public SyncData(Guid clientId, string clientName, List<CameraImage>? images, SyncOperationType type)
        {
            ClientId = clientId;
            ClientName = clientName;
            Images = images;
            Type = type;
        }
    }
}