using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
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
