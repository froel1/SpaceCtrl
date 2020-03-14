using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Data.Services.Device
{
    public class DeviceService
    {
        private readonly SpaceCtrlContext _dbContext;

        public DeviceService(SpaceCtrlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Database.Device> GetDevice(string key)
        {
            var device = _dbContext.Device.FirstOrDefaultAsync(x => x.Key == key);
            return await device;
        }

        public async Task<IEnumerable<Client>> GetGroupDetails(string ipCamKey)
        {
            var data = await _dbContext.Device
                .Include(x => x.Target)
                .ThenInclude(x => x.Client)
                .FirstOrDefaultAsync(x => x.Key == ipCamKey);

            return data.Target.Client;
        }
    }
}
