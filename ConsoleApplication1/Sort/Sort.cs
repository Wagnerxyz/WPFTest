using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm;

namespace MyExercise.Sort
{
    public class Sort
    {
        public void Run()
        {
            int[] iArrary = new int[] { 1, 5, 13, 6, 10, 55, 99, 2, 87, 12, 34, 75, 33, 47 };
            GnomeSort gs = new GnomeSort();
            InsertionSorter ii = new InsertionSorter();
            BubbleSorter bs = new BubbleSorter();
            SelectionSorter ss = new SelectionSorter();
            ShellSorter sh = new ShellSorter();
            gs.Sort(iArrary);
            for (int m = 0; m < iArrary.Length; m++)
                Console.Write("{0} ", iArrary[m]);
            Console.WriteLine();

            Console.ReadKey();
        }
        private static int Fibonacci(int a)
        {
            if (a == 1 || a == 2)
            {
                return 1;
            }
            else
            {
                return (Fibonacci(a - 2) + Fibonacci(a - 1));
            }
        }
    }


    public class BubbleSorter
    {
        public void Sort(int[] list)
        {
            int i, j, temp;
            bool done = false;
            j = 1;
            while ((j < list.Length) && (!done))
            {
                done = true;
                for (i = 0; i < list.Length - j; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        done = false;
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
                j++;
            }
        }
    }
    public class SelectionSorter
    {
        private int min;
        public void Sort(int[] list)
        {
            for (int i = 0; i < list.Length - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < list.Length; j++)
                {
                    if (list[j] < list[min])
                        min = j;
                }
                int t = list[min];
                list[min] = list[i];
                list[i] = t;
            }
        }
    }
    public class InsertionSorter
    {
        public void Sort(int[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int t = list[i];
                int j = i;
                while ((j > 0) && (list[j - 1] > t))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = t;
            }
        }
    }
    public class ShellSorter
    {
        public void Sort(int[] list)
        {
            int inc;
            for (inc = 1; inc <= list.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3)
            {
                for (int i = inc + 1; i <= list.Length; i += inc)
                {
                    int t = list[i - 1];
                    int j = i;
                    while ((j > inc) && (list[j - inc - 1] > t))
                    {
                        list[j - 1] = list[j - inc - 1];
                        j -= inc;
                    }
                    list[j - 1] = t;
                }
            }
        }
    }
}

