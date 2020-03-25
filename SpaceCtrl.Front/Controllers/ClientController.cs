using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Data.Models.Database;
using SpaceCtrl.Data.Services;
using SpaceCtrl.Front.Helpers;
using SpaceCtrl.Front.Models.Client;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.Controllers
{
    [Route("v1/person")]
    public class ClientController : Controller
    {
        private readonly PersonService _service;

        public ClientController(PersonService service)
        {
            _service = service;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddNewClientAsync([ModelBinder(BinderType = typeof(JsonModelBinder))]
            NewPersonModel person,
            IList<IFormFile> files)
        {
            await _service.AddAsync(person, files);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<ActionResult> RemoveClientAsync(RemoveClientModel model)
        {
            await _service.RemoveClientAsync(model.Id);
            return Ok();
        }
    }
}