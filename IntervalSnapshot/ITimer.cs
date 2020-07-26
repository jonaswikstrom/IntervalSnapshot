using System;
using System.Timers;

namespace IntervalSnapshot
{
    public interface ITimer
    {
        void Start();
        void Stop();

        void OnElapsed(Action action);
    }
}