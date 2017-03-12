using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise
{
    class DerivedTest
    {
        public static void Run()
        {
            Bderived b = new Bderived();
            ABase a = b;
            Iinterface i = b;
            a.hello();
            b.hello();
            i.hello();
        }
        interface Iinterface
        {
            void hello();
        }

        class ABase : Iinterface
        {
            public void hello()
            {
                Console.WriteLine("Ahello");
            }
        }

        class Bderived : ABase
        {
            public new void hello()
            {
                Console.WriteLine("Bhello");
            }
        }
    }
}
