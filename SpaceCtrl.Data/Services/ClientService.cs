﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceCtrl.Data.Interfaces;
using SpaceCtrl.Data.Models;
using SpaceCtrl.Data.Models.Database;

namespace SpaceCtrl.Data.Services
{
    public class ClientService
    {
        private readonly SpaceCtrlContext _dbContext;
        private readonly ISpaceCtrlCamera _SpaceCtrl;

        public ClientService(SpaceCtrlContext dbContext, ISpaceCtrlCamera SpaceCtrl)
        {
            _dbContext = dbContext;
            _SpaceCtrl = SpaceCtrl;
        }

        public async Task AddNewAsync(Client client, List<string> images)
        {
            _dbContext.Client.Add(client);
            await _SpaceCtrl.SendNewObjectAsync(new CameraObject(client.Guid, images));
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveClientAsync(int clientId)
        {
            var client = await _dbContext.Client.FirstAsync(x => x.Id == clientId);
            await _SpaceCtrl.RemoveObjectAsync(client.Guid);
            client.IsActive = !client.IsActive;
            await _dbContext.SaveChangesAsync();
        }
    }

}