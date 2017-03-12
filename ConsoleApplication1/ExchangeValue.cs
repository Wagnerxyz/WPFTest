using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithm
{
    class ExchangeValue
    {
        static void Swap<T>(ref T a,ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        //static void Swap(ref int a,ref int b)
        //{
        //    Interlocked.Exchange(ref a, b);
        //}

        //static void Main()
        //{
        //    string a = "aaa", b = "bbb";
        //  //  int a = 2, b = 7;
        //    Console.WriteLine("a = {0}, b = {1}", a, b);
        //    Swap( a,b);
        //    Console.WriteLine("a = {0}, b = {1}", a, b);
        //    Console.ReadKey();
        //}
    }
}
