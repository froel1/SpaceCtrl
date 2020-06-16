using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Models.Settings;
using SpaceCtrl.Data.Database.DbObjects;
using DateTime = System.DateTime;
using Object = SpaceCtrl.Data.Database.DbObjects.Object;

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
            var objects = new List<Object>();
            var deviceId = (await _device.GetAsync(deviceKey))!.DeviceId;
            var personKeys = @object.Data.Keys.Select(Guid.Parse).ToList();

            var persons = await _dbContext.Person
                .Include(x => x.Group).ThenInclude(x => x.GroupShift)
                .Where(x => personKeys.Contains(x.Key) && x.IsActive).ToListAsync();

            Log.Information("Group date {date}", @object.Date);

            foreach (var (key, model) in @object.Data)
            {
                Log.Information("Saving Object {objectId}, with count {count}", key, model.PersonCount);
                var dbObject = new Object
                {
                    CreateDate = DateTime.Now,
                    DeviceId = deviceId,
                    ChannelId = @object.CameraId,
                    PersonKey = Guid.Parse(key),
                    FrameDate = DateTime.Parse(@object.Date),
                    Frame = await SaveImageAsync(model),
                    PersonCount = model.PersonCount
                };

                FillGroupEntry(persons, dbObject);

                objects.Add(dbObject);
            }

            await _dbContext.Object.AddRangeAsync(objects);
            await _dbContext.SaveChangesAsync();
        }

        private static void FillGroupEntry(IEnumerable<Person> persons, Object @object)
        {
            var person = persons.FirstOrDefault(x => x.Key == @object.PersonKey);

            var shift = person?.Group.GroupShift.OrderByDescending(x => x.WeekNumber).FirstOrDefault();
            if (shift is null || (DateTime.Now < shift.StartDate || DateTime.Now > shift.EndDate))
                return;

            shift.GroupEntry.Add(new GroupEntry
            {
                Direction = (int)Direction.In,
                CreateDate = @object.FrameDate,
                PersonId = person.Id,
                FrameId = @object.Frame.Id
            });
        }

        private async Task<Frame> SaveImageAsync(DataModel model)
        {
            var id = Guid.NewGuid();
            var bytes = Convert.FromBase64String(model.Base64Image);
            var path = Path.Combine(_settings.Image.BasePath, $"{id}{model.ImageType}");

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