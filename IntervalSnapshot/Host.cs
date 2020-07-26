using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntervalSnapshot
{
    public class Host : IHostedService
    {
        private readonly ILogger<Host> logger;
        private readonly ITimer timer;
        private readonly IFileHandler fileHandler;

        public Host(ILogger<Host> logger, ITimer timer, ISnapshotProvider snapshotProvider, IFileHandler fileHandler)
        {
            this.logger = logger;
            this.timer = timer;
            this.fileHandler = fileHandler;

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
            logger.LogInformation("Starting host");
            timer.Start();
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Stop();
            return Task.CompletedTask;
        }
    }
}