using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Data.Models.Database;

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
            var objects = new List<Data.Models.Database.Object>();
            var deviceId = (await _device.GetAsync(deviceKey))!.DeviceId;

            var data = @object.Data
                .GroupBy(x => x.Key, y => y.Value)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

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

                var obj = new Data.Models.Database.Object
                {
                    CreateDate = DateTime.Now,
                    DeviceId = deviceId,
                    Direction = (int)@object.Direction,
                    PersonKey = Guid.Parse(key),
                    ImageDate = DateTime.Parse(@object.Date),
                    Image = await SaveImageAsync(model)
                };

                objects.Add(obj);
            }

            await _dbContext.Object.AddRangeAsync(objects);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Image> SaveImageAsync(DataModel model)
        {
            var id = Guid.NewGuid();
            var bytes = Convert.FromBase64String(model.Base64Image);
            var path = Path.Combine(_settings.ImagePath, $"{id}{model.ImageType}");

            await File.WriteAllBytesAsync(path, bytes);

            return new Image()
            {
                Id = id,
                Type = model.ImageType,
                CreateDate = DateTime.Parse(model.Date)
            };
        }
    }
}