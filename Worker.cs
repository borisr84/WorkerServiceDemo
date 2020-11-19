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
            _logger.LogInformation("Starting service...");
            _httpClient = new HttpClient();
            _logger.LogInformation("Service is started");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping service...");
            _httpClient.Dispose();
            _logger.LogInformation("Service is stopped");

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
                        _logger.LogInformation("Google site is up and running");
                    }
                    else
                    {
                        _logger.LogError("Google is unreachable");
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
