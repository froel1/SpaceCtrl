﻿using System;
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

        [HttpPost("add")]
        public async Task<ActionResult> AddNewClientAsync([ModelBinder(BinderType = typeof(JsonModelBinder))]
            NewClientModel client,
            IList<IFormFile> files)
        {
            await _service.AddAsync(client, files);
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