namespace TfsPoller
{
    public interface ITfsConfiguration
    {
        string TfsUrl { get;}
        string TfsUserName { get; }
        string TfsPassword { get; }
        string TfsTeamProjectName { get; }
    }
}