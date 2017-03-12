using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise.Eventtest
{
    class Person
    {
        public void Go(object o,EventArgs e)
        {
            Console.WriteLine($"{nameof(Person)} is runnning.事件注册成功");
        }
        public void Go()
        {
            Console.WriteLine($"{nameof(Person)} is runnning.事件注册成功");
        }
    }
}
