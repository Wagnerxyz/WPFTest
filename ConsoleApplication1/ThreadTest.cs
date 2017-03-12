using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class ThreadTest
    {

        internal class ThreadObj
        {
            public ManualResetEvent signalComplete { get; set; }
            public int TaskItem { get; set; }
        }

        static void ThreadWork(object o)
        {
            ThreadObj obj = (ThreadObj)o;
            Thread.Sleep(5000);
            obj.signalComplete.Set();
        }
        public static void Run()
        {
            // Using new .net 4.0 Task
            Stopwatch watch = new Stopwatch();
            watch.Start();
            System.Collections.Concurrent.ConcurrentBag<Task> tasks = new System.Collections.Concurrent.ConcurrentBag<Task>();
            Parallel.For(0, 60, i =>
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    System.Threading.Thread.Sleep(5000);
                });
                tasks.Add(t);
            });
            Console.WriteLine("Waiting for task to finish");
            Task.WaitAll(tasks.ToArray());
            watch.Stop();
            Console.WriteLine("Complete(Tasks) : Time " + watch.ElapsedMilliseconds.ToString());



            //// Using Thread 
            watch.Reset();
            watch.Start();
            System.Collections.Concurrent.ConcurrentBag<ManualResetEvent> tasksThreads = new System.Collections.Concurrent.ConcurrentBag<ManualResetEvent>();
            Parallel.For(0, 60, i =>
            {
                ManualResetEvent signal = new ManualResetEvent(false);
                tasksThreads.Add(signal);
                Thread t = new Thread(new ParameterizedThreadStart(ThreadWork));
                t.Start(new ThreadObj() { signalComplete = signal, TaskItem = i });
            });
            Console.WriteLine("Waiting for task to finish");
            WaitHandle.WaitAll(tasksThreads.ToArray());
            watch.Stop();
            Console.WriteLine("Complete(Threads) : Time " + watch.ElapsedMilliseconds.ToString());


            //// Using ThreadPool
            watch.Reset();
            watch.Start();
            System.Collections.Concurrent.ConcurrentBag<ManualResetEvent> tasksThreadPools = new System.Collections.Concurrent.ConcurrentBag<ManualResetEvent>();
            Parallel.For(0, 60, i =>
            {
                ManualResetEvent signal = new ManualResetEvent(false);
                tasksThreadPools.Add(signal);
                ThreadObj obj = new ThreadObj() { signalComplete = signal, TaskItem = i };
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadWork), obj);
            });
            Console.WriteLine("Waiting for task to finish");
            WaitHandle.WaitAll(tasksThreadPools.ToArray());
            watch.Stop();
            Console.WriteLine("Complete(ThreadPool) : Time " + watch.ElapsedMilliseconds.ToString());
            Console.ReadLine();
        }

    }


}
