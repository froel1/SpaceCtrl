using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Api.Controllers
{
    [ApiController]
    [Route("Clients")]
    public class ClientController : ControllerBase
    {
        private readonly SpaceCtrlContext _dbContext;

        public ClientController(SpaceCtrlContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Index()
        {
            var data = await _dbContext.Device.FirstOrDefaultAsync();
            return Ok(data?.Name ?? "works get");
        }

        [HttpGet("get")]
        public ActionResult<string> Get(List<Guid> ids)
        {

            return Ok("asd");
        }
    }
}
