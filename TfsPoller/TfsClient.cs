using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace TfsPoller
{
    public class TfsClient
    {
        private readonly ITfsConfiguration _configuration;

        public TfsClient(ITfsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<BuildState> GetLatestBuildStates()
        {
            using (TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(_configuration.TfsUrl), new NetworkCredential(_configuration.TfsUserName, _configuration.TfsPassword)))
            {
                IBuildServer buildServer = tfs.GetService<IBuildServer>();
                ITestManagementService testServer = tfs.GetService<ITestManagementService>();
                var defs = buildServer.QueryBuildDefinitions(_configuration.TfsTeamProjectName);
                return defs
                    .Select(def => GetLatestBuildDetails(buildServer, def))
                    .Where(build => build != null)
                    .Select(build => new BuildState(build.BuildDefinition.Name, build.Status, build.FinishTime, build.Uri.AbsoluteUri, build.RequestedFor,GetTestResults(testServer, build.Uri)))
                    .ToList();

            }
        }

        private IBuildDetail GetLatestBuildDetails(IBuildServer buildServer, IBuildDefinition def)
        {
            IBuildDetailSpec spec = buildServer.CreateBuildDetailSpec(_configuration.TfsTeamProjectName, def.Name);
            spec.MaxBuildsPerDefinition = 1;
            spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            var builds = buildServer.QueryBuilds(spec);
            return builds.Builds.FirstOrDefault();
        }

        private TestStatistics GetTestResults(ITestManagementService tms, Uri uri)
        {
            var testRuns = tms.GetTeamProject(_configuration.TfsTeamProjectName).TestRuns.ByBuild(uri);
            var tests = testRuns.FirstOrDefault();
            if (tests != null)
            {
                return new TestStatistics(tests.Statistics.PassedTests, tests.Statistics.FailedTests, tests.Statistics.TotalTests);
            }
            return new TestStatistics();
        }
    }
}
