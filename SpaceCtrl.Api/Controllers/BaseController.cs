using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Api.Attributes;

namespace SpaceCtrl.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        [DeviceKey, Required]
        [FromHeader(Name = "DeviceKey")]
        public Guid DeviceKey { get; set; }
    }
}
