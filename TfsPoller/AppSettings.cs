using System.Configuration;
using Microsoft.WindowsAzure;

namespace TfsPoller
{
    internal class AppSettings : ITfsConfiguration, ISlackConfiguration, ITfsPollerConfiguration
    {
        public string TfsUrl => GetAppsettingsString("tfs.url");
        public string TfsUserName => GetAppsettingsString("tfs.username");
        public string TfsPassword => GetAppsettingsString("tfs.password");
        public string TfsTeamProjectName => GetAppsettingsString("tfs.teamprojectname");
        public string SlackWebHookUrl => GetAppsettingsString("slack.webhookurl");
        public string SlackUserName => GetAppsettingsString("slack.username") ?? "TFS Build Bot";
        public string SlackDefaultChannel => GetAppsettingsString("slack.defaultchannel") ?? "@mads";
        public string SlackFailedChannel => GetAppsettingsString("slack.failedchannel") ?? "@mads";
        public string SlackFailedIcon => GetAppsettingsString("slack.failedicon") ?? ":name_badge:";
        public string SlackDefaultIcon => GetAppsettingsString("slack.defaulticon") ?? ":white_check_mark:";

        public int PollIntervallMinutes {

            get
            {
                int parsed;
                return int.TryParse(GetAppsettingsString("pollintervallminutes"), out parsed) ? parsed : 5;
            }
    }

    private string GetAppsettingsString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        
    }
}