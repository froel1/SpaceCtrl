using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Models.Camera;
using SpaceCtrl.Api.Models.Client;
using SpaceCtrl.Api.Models.Object;
using SpaceCtrl.Data.Models.Database;
using SpaceCtrl.Data.Services.Device;

namespace SpaceCtrl.Api.Controllers
{
    [ApiController]
    [Route("Device")]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceService _service;

        public DeviceController(DeviceService service)
        {
            _service = service;
        }

        [HttpGet("get")]
        public async Task<ActionResult<Device>> Get(string key) => await _service.GetDevice(key);

        /*[HttpPost]
        public async Task<ActionResult> AddPerson([FromBody] ObjectModel model)
        {
            return Ok();
        }*/

        [HttpPost]
        public async Task<ActionResult> CreatePerson([FromBody] NewClientModel model)
        {
            return Ok();
        }
    }
}
