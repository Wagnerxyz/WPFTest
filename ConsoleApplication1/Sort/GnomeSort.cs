using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class GnomeSort
    {
       public void Sort(int[] list)
        {

            int i = 0;   
            while (i < list.Length) {
      
                if (i == 0 || list[i-1] <= list[i])
                    i++;  
                else 
                {
                    int tmp = list[i];
                    list[i] = list[i-1];
                    list[i - 1] = tmp;// ar[--i] = tmp;
                    i--;
                }

  
            }
        }
    }
}
