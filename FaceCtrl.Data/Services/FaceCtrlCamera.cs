using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Models;

namespace SpaceCtrl.Data.Services
{
    public class FaceCtrlCamera : IFaceCtrlCamera
    {
        private readonly List<HttpClient> _ipCameras = new List<HttpClient>();
        public FaceCtrlCamera(IConfiguration configuration)
        {
            InitCameras(configuration);
        }

        public async Task SendNewObjectAsync(CameraObject @object)
        {

            var tasks = _ipCameras.Select(ipCamera => ipCamera.PostAsync(
                "update",
                new StringContent(@object.ToJson(), Encoding.UTF8, "application/json")
            )).ToList();

            await Task.WhenAll(tasks);
        }
        public async Task RemoveObjectAsync(Guid objectId)
        {
            var tasks = _ipCameras.Select(ipCamera =>
            {
                var model = JsonConvert.SerializeObject(objectId);

                return ipCamera.PostAsync(
                    "object/remove",
                    new StringContent(model, Encoding.UTF8, "application/json")
                );
            }).ToList();

            await Task.WhenAll(tasks);
        }

        private void InitCameras(IConfiguration configuration)
        {
            var cameraSettings = new FaceCtrlCameraSettings();
            configuration.GetSection("faceCtrlCamera").Bind(cameraSettings);
            foreach (var ipCamera in cameraSettings.IpCameras)
            {
                var baseUri = new Uri($"{cameraSettings.ServerAddress}:{ipCamera.Port}/");
                _ipCameras.Add(new HttpClient { BaseAddress = baseUri });
            }
        }

    }
}
