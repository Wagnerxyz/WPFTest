using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class ShellSortTest
    {// array of integers to hold values
        private int[] a = {6, 3, 2, 5, 1, 7};

        // number of elements in array
        private int x=6;

        // Shell Sort Algorithm
        public void sortArray()
        {
            int i, j, increment, temp;

            increment = 3;

            while (increment > 0)
            {
                for (i = 0; i < x; i++)
                {
                    j = i;
                    temp = a[i];

                    while ((j >= increment) && (a[j - increment] > temp))
                    {
                        a[j] = a[j - increment];
                        j = j - increment;
                    }

                    a[j] = temp;
                }

                if (increment / 2 != 0)
                {
                    increment = increment / 2;
                }
                else if (increment == 1)
                {
                    increment = 0;
                }
                else
                {
                    increment = 1;
                }
            }
            foreach (var i1 in a)
            {
                Console.WriteLine(i1);
            }
        }
    }
}
