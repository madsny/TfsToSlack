using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.TeamFoundation.Build.Client;
using Newtonsoft.Json;

namespace TfsPoller
{
    internal class SlackChangeNotifier : IChangeNotifier
    {
        private readonly ISlackConfiguration _configuration;

        public SlackChangeNotifier(ISlackConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Notify(BuildState buildState)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.SlackWebHookUrl);

                    var message = new SlackMessage
                    {
                        text = buildState.ToString(),
                        channel = GetChannel(buildState.Status),
                        icon_emoji = GetIconEmoji(buildState.Status),
                        username = _configuration.SlackUserName
                    };

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

                    var result =
                        client.PostAsync("", content).Result;
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(resultContent);
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to notify slack", e);
            }
        }

        private string GetChannel(BuildStatus buildStatus)
        {
            if (buildStatus == BuildStatus.Failed)
                return _configuration.SlackFailedChannel;
            return _configuration.SlackDefaultChannel;
        }

        private string GetIconEmoji(BuildStatus buildStatus)
        {
            if (buildStatus == BuildStatus.Failed)
                return _configuration.SlackFailedIcon;
            return _configuration.SlackDefaultIcon;
        }

        private class SlackMessage
        {
            public string text { get; set; }
            public string channel { get; set; }
            public string icon_emoji { get; set; }
            public string username { get; set; }
        }
    }
}