using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceCtrl.Front.Helpers;
using SpaceCtrl.Front.Models.Client;
using SpaceCtrl.Front.Models.Common;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.Controllers
{
    [Route("v1/client")]
    public class ClientController : Controller
    {
        private readonly ClientService _service;

        public ClientController(ClientService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<ClientDetails>> GetClientAsync(int id) => await _service.GetAsync(id);

        [HttpGet("list")]
        public async Task<ActionResult<PagedList<ClientDetails>>> GetClientsAsync([FromQuery] PaginationWithFilter<ClientFilterModel> model) =>
            await _service.GetAsync(model);

        [HttpGet("types")]
        public async Task<ActionResult<List<Dropdown>>> GetClientTypesAsync()
        {
            var data = new List<Dropdown>
            {
                new Dropdown(1, "test 1"),
                new Dropdown(2, "test 2"),
                new Dropdown(3, "test 3"),
                new Dropdown(4, "test 4"),
            };

            return await Task.FromResult(data);
        }

        [HttpGet("groups")]
        public async Task<List<Dropdown>> GetGroupsAsync() => await _service.GetGroupsAsync();

        [HttpPut("add")]
        public async Task<ActionResult> AddNewClientAsync([ModelBinder(BinderType = typeof(JsonModelBinder))]
            NewClientModel client,
            IList<IFormFile> files)
        {
            await _service.AddAsync(client, files);
            return Ok();
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveClientAsync(RemoveClientModel model)
        {
            await _service.RemoveClientAsync(model.Id);
            return Ok();
        }
    }
}