using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Options;
using Serilog;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Data.Database.DbObjects;

namespace SpaceCtrl.Api.Services
{
    public class ObjectService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly DeviceService _device;
        private readonly AppSettings _settings;

        public ObjectService(SpaceCtrlContext dbContext, IOptions<AppSettings> options, DeviceService device)
        {
            _dbContext = dbContext;
            _device = device;
            _settings = options.Value;
        }

        public async Task SaveObjectAsync(CameraObject @object, Guid deviceKey)
        {
            var objects = new List<Data.Database.DbObjects.Object>();
            var deviceId = (await _device.GetAsync(deviceKey))!.DeviceId;

            var timer = new Stopwatch();
            timer.Start();
            Log.Information("Group date {date}", @object.Date);
            foreach (var (key, model) in @object.Data)
            {
                /*Image image;
                try
                {
                    image = await SaveImageAsync(model);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }*/
                Log.Information("Saving Object {objectId}, with count {count}", key, model.PersonCount);

                objects.Add(new Data.Database.DbObjects.Object
                {
                    CreateDate = DateTime.Now,
                    DeviceId = deviceId,
                    ChannelId = @object.CameraId,
                    Direction = (int)@object.Direction,
                    PersonKey = Guid.Parse(key),
                    FrameDate = DateTime.Parse(@object.Date),
                    Frame = await SaveImageAsync(model),
                    PersonCount = model.PersonCount
                });
            }

            timer.Stop();
            Log.Debug($"Saved in {timer.Elapsed}\n\n");

            await _dbContext.Object.AddRangeAsync(objects);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Frame> SaveImageAsync(DataModel model)
        {
            var id = Guid.NewGuid();
            var bytes = Convert.FromBase64String(model.Base64Image);
            var path = Path.Combine(_settings.ImagePath, $"{id}{model.ImageType}");

            await File.WriteAllBytesAsync(path, bytes);

            return new Frame
            {
                Id = id,
                Type = model.ImageType,
                CreateDate = DateTime.Parse(model.Date)
            };
        }
    }
}