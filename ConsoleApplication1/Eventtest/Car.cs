using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise
{
    class Car
    {
        public void Go(object o, EventArgs e)
        {
            Console.WriteLine($"{nameof(Car) } is runnning.事件注册成功");
        }
        public void Go()
        {
            Console.WriteLine($"{nameof(Car)} is runnning.事件注册成功");
        }
    }
}
