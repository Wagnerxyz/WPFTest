using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplicau8t5u
{
    class MutexExample
    {
        private static Mutex mtx = new Mutex(true);
        static void Go()
        {
            Thread.CurrentThread.Name = "Main  ";
            Thread worker = new Thread(ThreadEntry);
            worker.Name = "worker";
            worker.Start();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            ThreadEntry();
        }
        private static void ThreadEntry()
        {
            var bool1 = mtx.WaitOne();
            string name = Thread.CurrentThread.Name;
            Console.WriteLine(String.Format("{0}:  {1}", name, DateTime.Now.Millisecond));
            mtx.ReleaseMutex();
            mtx.ReleaseMutex();//开始主线程拥有，要释放两次，子线程才能拿到
        }

        static void OneAtATimePlease()
        {
            // Naming a Mutex makes it available computer-wide. Use a name that's
            // unique to your company and application (e.g., include your URL).

            using (var mutex = new Mutex(true, "oreilly.com OneAtATimeDemo"))
            {
                // Wait a few seconds if contended, in case another instance
                // of the program is still in the process of shutting down.

                if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
                {
                    Console.WriteLine("Another app instance is running. Bye!");
                    return;
                }
                RunProgram();
            }
        }

        static void RunProgram()
        {
            Console.WriteLine("Running. Press Enter to exit");
            Console.ReadLine();
        }
    }
}
