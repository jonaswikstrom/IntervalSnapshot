using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntervalSnapshot
{
    public class Host : IHostedService
    {
        private readonly ILogger<Host> logger;
        private readonly ITimer timer;
        private readonly IFileHandler fileHandler;
        private readonly IOptions<Settings> settings;

        public Host(ILogger<Host> logger, ITimer timer, 
            ISnapshotProvider snapshotProvider, 
            IFileHandler fileHandler, IOptions<Settings> settings)
        {
            this.logger = logger;
            this.timer = timer;
            this.fileHandler = fileHandler;
            this.settings = settings;

            timer.OnElapsed(async () =>
            {
                using (var stream = await snapshotProvider.GetSnapshot())
                {
                    fileHandler.SaveStream(stream);
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Start();

            logger.LogInformation("Host started");
            logger.LogInformation($"Interval: {settings.Value.Interval.TotalMilliseconds} ms");
            logger.LogInformation($"Snapshot url: {settings.Value.SnapshotUrl}");
            logger.LogInformation($"Root directory: {settings.Value.RootDirectory}");
            logger.LogInformation($"Image file name: {settings.Value.ImageFileName}");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Stop();
            return Task.CompletedTask;
        }
    }
}