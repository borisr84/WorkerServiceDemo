using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace WorkerServiceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggerFactory => loggerFactory.AddEventLog().AddConsole())
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<EventLogSettings>(options =>
                    {
                        options.SourceName = "WSDemo";
                        options.LogName = "WSWatchdogDemo";
                        //options.Filter = (x, y) => y >= LogLevel.Warning;
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
