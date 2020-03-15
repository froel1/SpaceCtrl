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
    [Route("v1/client")]
    public class ClientController : Controller
    {
        private readonly ClientService _service;

        public ClientController(ClientService service)
        {
            _service = service;
        }

        [HttpPost("addNew")]
        public async Task<ActionResult> AddNewClientAsync([ModelBinder(BinderType = typeof(JsonModelBinder))]
            NewClientModel client,
            IList<IFormFile> files)
        {
            var newClient = new Client
            {
                Guid = Guid.NewGuid(),
                FirstName = client.FirstName,
                LastName = client.LastName,
                CreateDate = DateTime.Now,
                TargetId = 1,
                IsActive = true
            };

            var base64Files = await files.ToBase64StringAsync();
            await _service.AddNewAsync(newClient, base64Files);

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