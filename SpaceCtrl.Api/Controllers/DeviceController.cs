using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Api.Services;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api.Controllers
{
    [ApiController]
    [Route("v1/device")]
    public class DeviceController : BaseController
    {
        private readonly DeviceService _service;

        public DeviceController(DeviceService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task AddNewCameraAsync(DeviceModel device) => await _service.AddAsync(device);

        [HttpGet("list")]
        public async Task<ActionResult<List<DeviceModel>>> GetDevicesAsync() => await _service.GetDevicesAsync();

        [HttpGet("syncData")]
        public async Task<ActionResult<List<SyncData>>> GetSyncDetailsAsync() => await _service.GetSyncDataAsync();
    }
}