using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntervalSnapshot
{
    public class Timer : ITimer
    {
        private readonly ILogger<Timer> logger;
        private readonly IOptions<Settings> settings;
        private readonly System.Timers.Timer timer;
        private Action action;

        public Timer(ILogger<Timer> logger, IOptions<Settings> settings)
        {
            this.logger = logger;
            this.settings = settings;

            // ReSharper disable once NotResolvedInText
            if (Math.Abs(settings.Value.Interval.TotalMilliseconds) < 1000) throw new ArgumentOutOfRangeException("Interval must be higher than 1000 ms"); 

            timer = new System.Timers.Timer(settings.Value.Interval.TotalMilliseconds);
            logger.LogInformation($"Interval timer initiated for {settings.Value.Interval.TotalMilliseconds} ms");

            timer.Elapsed += (s, o) => { action?.Invoke(); };
        }

        public void Start()
        {
            logger.LogInformation("Starting timer");
            timer.Start();
        }

        public void Stop()
        {
            logger.LogInformation("Stopping timer");
            timer.Stop();
        }

        public void OnElapsed(Action elapsedAction)
        {
            action = elapsedAction;
        }
    }
}