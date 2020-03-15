using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceCtrl.Api.Models;

namespace SpaceCtrl.Api.Services
{
    public class DeviceCache
    {
        private readonly ConcurrentDictionary<Guid, DeviceModel> _devices;

        public DeviceCache()
        {
            _devices = new ConcurrentDictionary<Guid, DeviceModel>();
        }

        public bool Exist(Guid key) => _devices.TryGetValue(key, out var _);

        public void Add(Guid key, DeviceModel device) => _devices.TryAdd(key, device);

        public DeviceModel? Get(Guid key) => _devices.TryGetValue(key, out var device) ? device : null;

        public bool TryGet(Guid key, out DeviceModel? device) => _devices.TryGetValue(key, out device);
    }
}
