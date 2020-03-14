using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api.Services
{
    public class DeviceService
    {
        private readonly SpaceCtrlContext _dbContext;

        public DeviceService(SpaceCtrlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(DeviceModel device)
        {
            var newDevice = new Device
            {
                Key = device.Key,
                Name = device.Name ?? string.Empty,
                TargetId = device.TargetId
            };
            await _dbContext.Device.AddAsync(newDevice);
            await _dbContext.SaveChangesAsync();
        }

        /*public async Task<Models.Database.Device> GetDevice(string key)
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
        }*/
        public async Task<IEnumerable<DeviceModel>> GetDevicesAsync() => await _dbContext.Device.Select(x => new DeviceModel
        {
            Name = x.Name,
            OrderIndex = x.OrderIndex,
            Key = x.Key
        }).ToListAsync();
    }
}
