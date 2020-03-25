using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Models.ClientSync;
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
                Name = device.Name ?? string.Empty
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

        public async Task<List<SyncData>> GetSyncDataAsync()
        {
            var syncDetails = await GetSyncDetailsAsync();
            var data = new List<SyncData>();

            foreach (var (clientId, clientSyncDetails) in syncDetails)
            {
                List<CameraImage> images = null;
                if (clientSyncDetails.SyncDetails.Type == SyncOperationType.NewClient)
                    images = await ReadImagesAsync(clientSyncDetails!.SyncDetails);

                data.Add(new SyncData(clientId, images, clientSyncDetails!.SyncDetails!.Type));
            }

            await _dbContext.SaveChangesAsync();
            return data;
        }

        private static async Task<List<CameraImage>> ReadImagesAsync(SyncDetails syncDetails)
        {
            var cameraImages = new List<CameraImage>();

            foreach (var imageName in syncDetails.Images)
            {
                var path = Path.Combine(syncDetails.ImagePath, imageName);
                var imageData = await File.ReadAllBytesAsync(path);
                cameraImages.Add(new CameraImage
                {
                    Base64Data = Convert.ToBase64String(imageData),
                    Name = imageName
                });
            }

            return cameraImages;
        }

        private async Task<Dictionary<Guid, PersonSyncDetails>> GetSyncDetailsAsync()
        {
            var persons = await _dbContext.Person.Where(x => x.SyncRequestedAt.HasValue)
                .OrderByDescending(x => x.SyncRequestedAt)
                .AsTracking().Take(5).ToListAsync();

            var syncDetails = new Dictionary<Guid, PersonSyncDetails>();

            foreach (var person in persons)
            {
                person.SyncRequestedAt = null;
                var syncDetail = person.SyncDetails.DeserializeToObject<PersonSyncDetails>();
                if (syncDetail is null)
                    continue;//TODO:cot ??
                syncDetails.Add(person.Key, syncDetail);
                person.SyncDetails = UpdateSyncDetails(syncDetail);
            }

            return syncDetails;
        }

        private static string UpdateSyncDetails(PersonSyncDetails syncDetails)
        {
            syncDetails.LastSyncDate = DateTime.Now;
            syncDetails.SyncHistory ??= new List<SyncDetails>();
            syncDetails.SyncHistory.Add(syncDetails.SyncDetails);
            syncDetails.SyncDetails.SyncDate = DateTime.Now;

            return syncDetails.ToJson();
        }

        public async Task<List<DeviceModel>> GetDevicesAsync() =>
            await _dbContext.Device.Select(DeviceToModel()).ToListAsync();

        private static Expression<Func<Device, DeviceModel>> DeviceToModel() => device => new DeviceModel
        {
            Name = device.Name,
            Key = device.Key
        };
    }
}