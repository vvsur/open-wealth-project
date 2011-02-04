using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Collections;

namespace OpenWealth.DataManager
{
    public static class TicksFileSaver
    {
        static ILog l = Core.GetLogger(typeof(TicksFileSaver).FullName);

        static List<SaveTask> queue = new List<SaveTask>();
        static Thread thread;
        static ManualResetEvent mre = new ManualResetEvent(false);
        static bool KeepRunning = true;

        static TicksFileSaver()
        {
            thread = new Thread(ThreadDo);
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Name = "TicksFileSaver";
            thread.Start();
        }

        public static void Add4Save(TicksFile ticksFile, IList<IBar> notGarbage)
        {
            lock (queue)
            {
                foreach (SaveTask st in queue)
                    if (st.ticksFile == ticksFile)
                    {
                        st.lastAdd = DateTime.Now;
                        return;
                    }
                queue.Add(new SaveTask(ticksFile, notGarbage));
            }
            mre.Set();
        }

        static void ThreadDo()
        {
            try
            {
                int queue_Count = 0;
                while (KeepRunning || (queue_Count > 0)) // TODO Error бесконечный цикл, иногда программа не выходила из него (см. ERROR ниже)
                {
                    lock (queue)
                    {
                        queue_Count = queue.Count;
                    }
                    if (queue_Count == 0)
                    {
                        mre.WaitOne(10000);
                        mre.Reset();         // ERROR похоже что она весела здесь
                    }
                    SaveTask task = null;
                    lock (queue)
                    {
                        foreach(SaveTask t in queue)
                            if ((t.NeedSave) || (!KeepRunning))
                            {
                                task = t;
                                queue.Remove(t);
                                break;
                            }
                    }
                    if (task == null)
                        Thread.Sleep(1000);
                    else
                    {
                        try
                        {
                            task.ticksFile.Save(task.notGarbage);
                        }
                        catch (Exception ex)
                        {
                            l.Error("Exception в task.ticksFile.Save ", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                l.Error("Exception в ThreadDo", ex);
            }
        }

        public static void Stop()
        {
            KeepRunning = false;
            mre.Set();
        }

        class SaveTask
        {
            public readonly TicksFile ticksFile;
            public readonly IList<IBar> notGarbage;
            internal DateTime lastAdd = DateTime.Now;
            readonly DateTime сreate  = DateTime.Now;

            public SaveTask(TicksFile ticksFile, IList<IBar> notGarbage)
            {
                this.ticksFile = ticksFile;
                this.notGarbage = notGarbage;
            }

            // Правила, определяющие, когда следует сохранять файл.
            TimeSpan waitTime = new TimeSpan(0, 0, 10);
            TimeSpan dontWait = new TimeSpan(0, 5, 0);
            internal bool NeedSave
            {
                get
                {
                    DateTime now = DateTime.Now;
                    return ((now - lastAdd) > waitTime) || ((now - сreate) > dontWait);
                }
            }

        }
    }
}
