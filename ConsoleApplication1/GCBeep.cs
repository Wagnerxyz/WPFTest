using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal sealed class GCBeep
    {
        ~GCBeep()
        { // This is the Finalize method 
            Console.Beep();
            // If the AppDomain isn’t unloading and if the process isn’t shutting down,
            // create a new object that will get finalized at the next collection. 
            if (!AppDomain.CurrentDomain.IsFinalizingForUnload() && !Environment.HasShutdownStarted)
                new GCBeep();
        }
    }
}
