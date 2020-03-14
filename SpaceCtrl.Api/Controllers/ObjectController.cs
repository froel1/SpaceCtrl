using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Services;

namespace SpaceCtrl.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("v1/object")]
    public class ObjectController : BaseController
    {
        private readonly ObjectService _service;

        public ObjectController(ObjectService service)
        {
            _service = service;
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return Ok("works");
        }

        [HttpPost("add")]
        public async Task AddNewObjectAsync(CameraObject @object) => await _service.AddAsync(@object);

    }
}
