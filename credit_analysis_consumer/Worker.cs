using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace credit_analysis_consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProcessRequestService _processRequestService;

        public Worker(ILogger<Worker> logger, IProcessRequestService processRequestService)
        {
            _logger = logger;
            _processRequestService = processRequestService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _processRequestService.ProcessRequestsFromQueue();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
