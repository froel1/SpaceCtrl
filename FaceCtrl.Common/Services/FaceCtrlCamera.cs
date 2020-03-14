using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FaceCtrl.Common.Interfaces;
using FaceCtrl.Common.Models;

namespace FaceCtrl.Common.Services
{
    public class FaceCtrlCamera : IFaceCtrlCamera
    {
        public Task SendNewObjectAsync(CameraObject @object) => Task.CompletedTask;
        public Task RemoveObjectAsync(Guid objectId) => Task.CompletedTask;
    }
}
