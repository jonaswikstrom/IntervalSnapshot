using System;

namespace IntervalSnapshot
{
    public class Settings
    {
        public TimeSpan Interval { get; set; }
        public string SnapshotUrl { get; set; }
        public string ImageFileName { get; set; }

        public string RootDirectory { get; set; }
    }
}