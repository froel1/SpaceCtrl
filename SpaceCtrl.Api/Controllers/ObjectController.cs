using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Services;

namespace SpaceCtrl.Api.Controllers
{
    [ApiController]
    [Route("v1/object")]
    public class ObjectController : BaseController
    {
        private readonly ObjectService _service;

        public ObjectController(ObjectService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task AddNewObjectAsync(CameraObject @object)
        {
            foreach (var (key, model) in @object.Data)
            {
                var personId = Guid.Parse(key);
                var frameDate = DateTime.Parse(model.Date);
                var imageName = $"{Guid.NewGuid()}{model.ImageType}";

                var bytes = Convert.FromBase64String(model.Base64Image);
                System.IO.File.WriteAllBytes("C:\\Users\\Jimbo\\Desktop\\Docker Images\\test.jpg", bytes);
            }
        }
    }
}