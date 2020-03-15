using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SpaceCtrl.ImageProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _settings;

        public Worker(ILogger<Worker> logger, IOptions<AppSettings> options)
        {
            _logger = logger;
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
