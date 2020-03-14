using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FaceCtrl.Common.Models;

namespace FaceCtrl.Common.Interfaces
{
    public interface IFaceCtrlCamera
    {
        Task SendNewObjectAsync(CameraObject @object);
        Task RemoveObjectAsync(Guid objectId);
    }
}
