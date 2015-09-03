namespace TfsPoller
{
    internal interface ISlackConfiguration
    {
        string SlackWebHookUrl { get; }
        string SlackUserName { get; }
        string SlackDefaultChannel { get;}
        string SlackFailedChannel { get; }
        string SlackFailedIcon { get; }
        string SlackDefaultIcon { get; }
    }
}