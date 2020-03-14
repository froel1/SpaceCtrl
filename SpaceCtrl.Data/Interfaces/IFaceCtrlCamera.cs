using System;
using System.Threading.Tasks;

namespace SpaceCtrl.Data.Interfaces
{
    public interface ISpaceCtrlCamera
    {
        Task RemoveObjectAsync(Guid objectId);
    }
}
