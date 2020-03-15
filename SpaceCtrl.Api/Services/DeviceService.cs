using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api.Services
{
    public class DeviceService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly DeviceCache _cache;

        public DeviceService(SpaceCtrlContext dbContext, DeviceCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
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

        public async Task<DeviceModel?> GetAsync(Guid key)
        {
            if (_cache.TryGet(key, out var device))
                return device;

            device = await _dbContext.Device.Select(DeviceToModel()).FirstOrDefaultAsync(x => x.Key == key);

            if (device != null)
                _cache.Add(device.Key, device);

            return device;
        }

        public async Task<bool> ExistAsync(Guid key)
        {
            if (_cache.Exist(key))
                return true;

            var device = await _dbContext.Device.Select(DeviceToModel()).FirstOrDefaultAsync(x => x.Key == key);
            if (device == null)
                return false;

            _cache.Add(device.Key, device);
            return true;
        }

        public async Task<IEnumerable<DeviceModel>> GetDevicesAsync() =>
            await _dbContext.Device.Select(DeviceToModel()).ToListAsync();

        private static Expression<Func<Device, DeviceModel>> DeviceToModel() => device => new DeviceModel
        {
            Name = device.Name,
            OrderIndex = device.OrderIndex,
            Key = device.Key
        };
    }
}
