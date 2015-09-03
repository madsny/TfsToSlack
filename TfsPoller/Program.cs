using System;
using System.Configuration;
using System.Threading;
using log4net.Config;
using Microsoft.TeamFoundation.Build.Client;
using Topshelf;

namespace TfsPoller
{
    class Program
    {
        static void Main(string[] args)
        {
            var appsettings = new AppSettings();
            var tfsClient = new TfsClient(appsettings);
            IChangeNotifier changeNotifier = new SlackChangeNotifier(appsettings);
            var changeDetector = new ChangeDetector(tfsClient, changeNotifier);

            HostFactory.Run(x =>
            {
                x.UseLog4Net();
                XmlConfigurator.Configure();
                x.Service<ChangePoller>(s =>
                {
                    s.ConstructUsing(name => new ChangePoller(changeDetector, appsettings));
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Polling TFS for builds");
                x.SetDisplayName("TfsToSlack");
                x.SetServiceName("TfsToSlack");
            });


            ////changeDetector.CheckForChanges();

            ////var timer = new Timer(state => changeDetector.CheckForChanges(), null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 2, 0));

            //var state = new BuildState("Bygg_CI_MASTER", BuildStatus.Succeeded, new DateTime(2010, 10, 10, 11, 42, 32), "abc", "Mads Nyborg", new TestStatistics(1, 0, 2));

            //changeNotifier.Notify(state);

            //Console.ReadKey();
        }
    }
}
