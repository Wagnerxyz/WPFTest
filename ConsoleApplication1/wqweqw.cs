using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise
{
    class A
    {
        public virtual void F()
        {
            Console.WriteLine("A.f");
        }

    }
    abstract  class B :A
    {
        public abstract override void F();
    }
}
