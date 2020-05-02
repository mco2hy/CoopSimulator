using System;
using System.Threading;

namespace CoopSimulator.Helper
{
    public class ThreadHelper
    {
        public static Thread ExecuteThread(Action action, bool join = false)
        {
            Thread thread = new Thread(() => action(), 1024 * 1024);
            thread.Start();

            if (join)
            {
                thread.Join();
            }

            return thread;
        }
    }
}