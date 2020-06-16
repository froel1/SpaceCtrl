using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Data.Database.DbObjects;
using SpaceCtrl.Data.Extensions;
using SpaceCtrl.Data.Helpers;
using SpaceCtrl.Data.Models.ClientSync;

namespace SpaceCtrl.Api.Services
{
    public class DeviceService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly DeviceCache _cache;
        private readonly AppSettings _settings;

        public DeviceService(SpaceCtrlContext dbContext, DeviceCache cache, IOptions<AppSettings> settings)
        {
            _dbContext = dbContext;
            _cache = cache;
            _settings = settings.Value;
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

            foreach (var (person, clientSyncDetails) in syncDetails)
            {
                if (clientSyncDetails?.SyncDetails?.Type is null)
                {
                    Log.Error("Something went wrong new sync request without details, client {id}", person.Id);
                    continue;
                }

                var images = clientSyncDetails.SyncDetails.Type switch
                {
                    SyncOperationType.NewClient => await ReadImagesAsync(person),
                    SyncOperationType.UpdateClient => await ReadImagesAsync(person),
                    SyncOperationType.DeleteClient => null,
                    _ => null
                };

                data.Add(new SyncData(person.Key, $"{person.FirstName} {person.LastName}", images, clientSyncDetails.SyncDetails.Type));
            }

            await _dbContext.SaveChangesAsync();
            return data;
        }

        private async Task<List<CameraImage>> ReadImagesAsync(Person person)
        {
            var cameraImages = new List<CameraImage>();

            foreach (var image in person.PersonImages)
            {
                var path = image.Frame.GetPath(_settings.Image, ImageType.User);
                var imageData = await File.ReadAllBytesAsync(path);

                cameraImages.Add(new CameraImage
                {
                    Base64Data = Convert.ToBase64String(imageData),
                    Name = $"{image.Frame.Id}{image.Frame.Type}"
                });
            }

            return cameraImages;
        }

        private async Task<Dictionary<Person, PersonSyncDetails>> GetSyncDetailsAsync()
        {
            var persons = await _dbContext.Person
                .Include(x => x.PersonImages).ThenInclude(x => x.Frame)
                .Where(x => x.SyncRequestedAt.HasValue)
                .OrderByDescending(x => x.SyncRequestedAt)
                .AsTracking().Take(5).ToListAsync();

            var syncDetails = new Dictionary<Person, PersonSyncDetails>();

            foreach (var person in persons)
            {
                person.SyncRequestedAt = null;
                var syncDetail = person.SyncDetails.DeserializeToObject<PersonSyncDetails>();

                if (syncDetail is null)
                {
                    Log.Error("Something went wrong new sync request without details, client {id}", person.Id);
                    continue;
                }

                syncDetails.Add(person, syncDetail);
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
            Key = device.Key,
            DeviceId = device.Id
        };
    }
}