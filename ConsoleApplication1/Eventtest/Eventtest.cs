using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise.Eventtest
{

    class Eventtest
    {
        public static void Run()
        {
            var car1 = new Car();
            var person1 = new Person();
            var light1 = new Light();
            light1.LightON += car1.Go;
            light1.LightON += person1.Go;
            light1.OnLightOn();
            //Console.ReadKey();
        }
        public static void EventTest()
        {
            var car1 = new Car();
            var person1 = new Person();
            var light1 = new Light();
            light1.LightON += car1.Go;
            light1.LightON += person1.Go;
            light1.OnLightOn();
            //Console.ReadKey();
        }
        public static void delegateTest()
        {
            var car1 = new Car();
            var person1 = new Person();
            var light1 = new Light();
            var dele = new lightdelegate(car1.Go);
            dele += car1.Go;
            dele += person1.Go;
            dele();
            //Console.ReadKey();
        }
    }
}