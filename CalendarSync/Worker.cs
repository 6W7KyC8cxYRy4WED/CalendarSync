using Azure.Core;
using Azure.Identity;
using CalendarSync.MicrosoftGraph;
using CalendarSync.Entities;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace CalendarSync
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IServiceScope serviceScope = _serviceProvider.CreateScope();
                IServiceProvider serviceProvider = serviceScope.ServiceProvider;
                DatabaseContext databaseContext = serviceProvider.GetService<DatabaseContext>();
                MicrosoftManager.Sync(databaseContext);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}