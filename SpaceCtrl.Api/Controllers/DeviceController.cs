using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Models;
using SpaceCtrl.Api.Services;

namespace SpaceCtrl.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("v1/device")]
    public class DeviceController : BaseController
    {
        private readonly DeviceService _service;

        public DeviceController(DeviceService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task AddNewCamera(DeviceModel device) => await _service.AddAsync(device);
    }
}