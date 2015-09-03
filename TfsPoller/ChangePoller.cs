using System;
using System.Reflection;
using System.Threading;
using log4net;

namespace TfsPoller
{
    public class ChangePoller
    {
        private readonly ChangeDetector _changeDetector;
        private readonly ITfsPollerConfiguration _configuration;
        private Timer _timer;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ChangePoller(ChangeDetector changeDetector, ITfsPollerConfiguration configuration)
        {
            _changeDetector = changeDetector;
            _configuration = configuration;
        }

        public void Start()
        {
            _timer = new Timer(state => _changeDetector.CheckForChanges(), null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, _configuration.PollIntervallMinutes, 0));
            Log.Info("Started");
        }

        public void Stop()
        {
            _timer?.Dispose();
            Log.Info("Stopped");
        }
    }
}
