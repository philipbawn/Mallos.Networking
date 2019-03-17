namespace Mallos.Networking
{
    using System.Threading;

    static class Helpers
    {
        public static void WaitUntilRunning(NetPeer netPeer)
        {
            int retries = 100;
            while (retries > 0 && !netPeer.Running)
            {
                Thread.Sleep(10);
                retries -= 1;
            }
        }
    }
}
