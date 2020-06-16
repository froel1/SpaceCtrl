using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SpaceCtrl.Front.Services;

namespace SpaceCtrl.Front.BackgroundServices
{
    public class GroupSchedulerHostedService : BackgroundService
    {
        private readonly GroupService _service;

        public GroupSchedulerHostedService(GroupService service)
        {
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextDay = DateTime.Now.AddDays(1).Date - DateTime.Now;
                try
                {
                    await _service.UpdateGroupScheduleAsync();
                    Console.WriteLine("works");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e); //TODO
                    throw;
                }

                await Task.Delay(nextDay, stoppingToken);
            }
        }
    }
}