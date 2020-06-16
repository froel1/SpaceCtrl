using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SpaceCtrl.Api.Attributes;
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
        public async Task<ActionResult> AddNewObjectAsync(CameraObject @object)
        {
            await _service.SaveObjectAsync(@object, DeviceKey);
            return Ok();
        }
    }
}