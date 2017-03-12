using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal static class FalseSharing
    {
        [StructLayout(LayoutKind.Explicit)]
        private class Data
        {
            // These two fields are adjacent and (most likely) in the same cache line
            [FieldOffset(0)]
            public Int32 field1;
            [FieldOffset(32)]
            public Int32 field2;
        }
        private const Int32 iterations = 100000000; // 100 million
        private static Int32 s_operations = 2;
        private static Int64 s_startTime;
        public static void Run()
        {
            // Allocate an object and record the start time
            Data data = new Data();
            s_startTime = Stopwatch.GetTimestamp();
            // Have 2 threads access their own fields within the structure
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 0));
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 1));
            // For testing, block the Main thread
            Console.ReadLine();
        }
        private static void AccessData(Data data, Int32 field)
        {
            // The threads in here each access their own field within the Data object
            for (Int32 x = 0; x < iterations; x++)
                if (field == 0) data.field1++; else data.field2++;
            // Whichever thread finishes last, shows the time it took
            if (Interlocked.Decrement(ref s_operations) == 0)
                Console.WriteLine("Access time: {0:N0}", Stopwatch.GetTimestamp() - s_startTime);
        }
    }
}
