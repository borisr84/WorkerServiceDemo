using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {ServiceName} service...", nameof(Worker));
            _httpClient = new HttpClient();
            _logger.LogInformation("Service {ServiceName} is started", nameof(Worker));

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {ServiceName} service...", nameof(Worker));
            _httpClient.Dispose();
            _logger.LogInformation("Service {ServiceName} is stopped", nameof(Worker));

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetAsync("https://www.google.co.il/");
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("{SiteName} site is up and running", "Google");
                    }
                    else
                    {
                        _logger.LogError("{SiteName} is unreachable", "Google");
                    }
                    await Task.Delay(2000, stoppingToken);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"Could not complete HTTP request - {ex.Message}");
                    await Task.Delay(2000, stoppingToken);
                }
            }
        }
    }
}
