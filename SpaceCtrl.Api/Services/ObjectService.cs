using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api.Services
{
    public class ObjectService
    {
        private readonly SpaceCtrlContext _dbContext;

        public ObjectService(SpaceCtrlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CameraObject model)
        {
            if (!DateTime.TryParse(model.Date, out var date))
                date = DateTime.Now;

            var @object = new Data.Models.Database.Object
            {
                DeviceKey = model.DeviceKey,
                Direction = (int)model.Direction,
                CreateDate = date,
                ObjectToClient = model.Objects.Select(x => new ObjectToClient
                {
                    ObjectGuid = x
                }).ToList(),
                Image = await SaveImageAsync(model.Image)
            };

            await _dbContext.Object.AddAsync(@object);
            await _dbContext.SaveChangesAsync();
        }

        private static async Task<Image> SaveImageAsync(CameraImage image) => await Task.FromResult(new Image
        {
            Name = Guid.NewGuid().ToString(),
            Type = "png"
        });
    }


}
