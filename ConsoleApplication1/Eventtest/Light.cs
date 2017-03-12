using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExercise.Eventtest
{
    delegate void lightdelegate();
    class Light
    { public event EventHandler LightON;
        public void OnLightOn()
        {
            if (LightON!=null)
            {
                this.LightON(null, null);
            }
        }
    }
}
