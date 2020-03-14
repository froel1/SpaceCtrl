using System;
using System.Threading.Tasks;
using SpaceCtrl.Data.Models;

namespace SpaceCtrl.Data.Interfaces
{
    public interface ISpaceCtrlCamera
    {
        Task SendNewObjectAsync(CameraObject @object);
        Task RemoveObjectAsync(Guid objectId);
    }
}
