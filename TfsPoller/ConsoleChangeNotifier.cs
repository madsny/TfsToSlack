using System;

namespace TfsPoller
{
    internal class ConsoleChangeNotifier : IChangeNotifier
    {
        public void Notify(BuildState buildState)
        {
            Console.WriteLine(buildState);
        }
    }
}