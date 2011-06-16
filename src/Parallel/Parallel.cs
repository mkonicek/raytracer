using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace ParallelExec
{
    public delegate void DoDelegate();

    public delegate void ForDelegate(int i);
    public delegate void ThreadDelegate();

	/// <summary>
	/// Parallel utils for .NET 2.0.
	/// </summary>
    public class Parallel
    {
        /// <summary>
        /// Parallel for loop. Invokes given action, passing arguments 
        /// fromInclusive - toExclusive on multiple threads.
        /// Returns when loop finished.
        /// </summary>
        public static void For(int fromInclusive, int toExclusive, ForDelegate action)
        {
            // ChunkSize = 1 makes items to be processed in order.
            // Bigger chunk size should reduce lock waiting time and thus
            // increase paralelism.
            int chunkSize = 4;

            // number of process() threads
            int threadCount = Environment.ProcessorCount;
            int cnt = fromInclusive - chunkSize;

            // processing function
            // takes next chunk and processes it using action
            ThreadDelegate process = delegate()
            {
                while (true)
                {
                    int cntMem = 0;
                    lock (typeof(Parallel))
                    {
                        // take next chunk
                        cnt += chunkSize;
                        cntMem = cnt;
                    }
                    // process chunk
                    // here items can come out of order if chunkSize > 1
                    for (int i = cntMem;  i < cntMem + chunkSize; ++i)
                    {
                        if (i >= toExclusive) return;
                        action(i);
                    }
                }
            };

            // launch process() threads
            IAsyncResult[] asyncResults = new IAsyncResult[threadCount];
            for (int i = 0; i < threadCount; ++i)
            {
                asyncResults[i] = process.BeginInvoke(null, null);
            }
            // wait for all threads to complete
            for (int i = 0; i < threadCount; ++i)
            {
                process.EndInvoke(asyncResults[i]);
            }
        }

        /// <summary>
        /// Fork - join. Invokes all actions on multiple threads.
        /// Returns when all finished.
        /// </summary>
        /// <param name="action"></param>
        public static void Do(params DoDelegate[] actions)
        {
            IAsyncResult[] results = new IAsyncResult[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                results[i] = actions[i].BeginInvoke(null, null);
            }
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].EndInvoke(results[i]);
            }
        }

        /// <summary>
        /// Starts given action on new thread. Does not wait for or notify when action ends.
        /// </summary>
        public static void Start(DoDelegate action)
        {
            /*WaitCallback waitCallBack = delegate(object state)
            {
                action();
            };
            ThreadPool.QueueUserWorkItem(waitCallBack);*/

            // background worker supports exception handling
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (object sender, DoWorkEventArgs e) => action();
            bw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                if (e.Error != null)
                {
                    throw e.Error;
                }
            };
            bw.RunWorkerAsync();      
        }
    }
}
