using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.TeamFoundation.Build.Client;

namespace TfsPoller
{
    public class ChangeDetector
    {
        private readonly TfsClient _tfsClient;
        private readonly IChangeNotifier _changeNotifier;
        private readonly Dictionary<string, string> _latestBuilds;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public ChangeDetector(TfsClient tfsClient, IChangeNotifier changeNotifier)
        {
            _tfsClient = tfsClient;
            _changeNotifier = changeNotifier;
            _latestBuilds = new Dictionary<string,string>();
        }

        public void CheckForChanges()
        {
            try
            {
                Log.Debug("Checking for changes");
                var buildStates = _tfsClient.GetLatestBuildStates();
                Log.InfoFormat("Found {0} build states", buildStates.Count);
                foreach (var buildState in buildStates.Where(BuildIsDone))
                {
                    string previousBuild;
                    _latestBuilds.TryGetValue(buildState.Name, out previousBuild);
                    if (!string.Equals(buildState.Uri, previousBuild) && buildState.FinishTime > DateTime.Now.AddMinutes(-15))
                    {
                        Log.DebugFormat("Notifying:\n {0}", buildState);
                        _changeNotifier.Notify(buildState);
                    }
                    _latestBuilds[buildState.Name] = buildState.Uri;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error while checking for changes", e);
            }
        }

        private static bool BuildIsDone(BuildState b)
        {
            return b.Status == BuildStatus.Failed || b.Status == BuildStatus.Succeeded;
        }
    }
}