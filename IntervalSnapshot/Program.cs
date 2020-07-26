using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntervalSnapshot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, true)
                .AddEnvironmentVariables()
                .Build();

            var hostBuilder = new HostBuilder().ConfigureServices(p =>
            {
                p.AddLogging(loggingBuilder => loggingBuilder.AddConsole())
                    .Configure<Settings>(configuration)
                    .AddSingleton<ITimer, Timer>()
                    .AddSingleton<ISnapshotProvider, SnapshotProvider>()
                    .AddSingleton<IFileHandler, FileHandler>()
                    .AddHostedService<Host>();

            });

            Console.WriteLine("Starting host for IntervalSnapshot");
            await hostBuilder.RunConsoleAsync();
        }
    }
}
