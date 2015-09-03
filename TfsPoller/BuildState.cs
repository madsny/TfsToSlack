using System;
using Microsoft.TeamFoundation.Build.Client;

namespace TfsPoller
{
    public class BuildState
    {
        public string Name { get; private set; }
        public BuildStatus Status { get; private set; }
        public DateTime FinishTime { get; private set; }
        public string Uri { get; private set; }
        public string RequestedBy { get; private set; }
        public TestStatistics TestStatistics { get; private set; }

        public BuildState(string name, BuildStatus status, DateTime finishTime, string uri, string requestedBy, TestStatistics testStatistics)
        {
            Uri = uri;
            Name = name;
            Status = status;
            FinishTime = finishTime;
            Uri = uri;
            RequestedBy = requestedBy;
            TestStatistics = testStatistics;
        }

        public override string ToString()
        {
            return string.Format("{0}\nStatus: {1}\nRequested by: {2}\nFinished: {3}\nTests: {4}\n", Name, Status, RequestedBy, FinishTime, TestStatistics);
        }
    }
}