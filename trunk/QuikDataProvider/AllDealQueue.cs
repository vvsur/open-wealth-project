using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenWealth.QuikDataProvider.DDE
{
    public static class AllDealQueue
    {
        static ILog l = Core.GetLogger(typeof(AllDealQueue).FullName);

        static List<byte[]> queue = new List<byte[]>();
        static List<byte[]> workQueue = new List<byte[]>();
        static Object locker = new Object();
        static ManualResetEvent mre = new ManualResetEvent(false);

        static Thread thread;

        static AllDealQueue()
        {
            thread = new Thread(ThreadDo);
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public static void Add(byte[] data)
        {
            lock (locker)
            {
                queue.Add(data);
                mre.Set();
            }
        }

        static void ThreadDo()
        {
            //TODO Как определить что WL завершается?
            //TODO Добавить в IPlugin метод остановки
            while (true)
            {
                try
                {
                    mre.WaitOne();
                    lock (locker)
                    {
                        if (queue.Count > 0)
                        {
                            List<byte[]> tmp = queue;
                            queue = workQueue;
                            workQueue = tmp;
                        }
                        mre.Reset();
                    }

                    foreach (byte[] data in workQueue)
                        XLTableWraper.GetDeals(data);
                    workQueue.Clear();
                }
                catch (Exception ex)
                {
                    l.Error("Exception в ThreadDo", ex);
                }
            }

        }
    }
}
