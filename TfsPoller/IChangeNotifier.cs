namespace TfsPoller
{
    public interface IChangeNotifier
    {
        void Notify(BuildState buildState);
    }
}