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

        private static async Task<Image> SaveImageAsync(CameraImage image) => await Task.FromResult(new Image
        {
            Type = "png"
        });
    }
}