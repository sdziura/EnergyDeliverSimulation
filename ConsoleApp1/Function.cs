using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Function
    {  
        const int Tmin = 70, Tmax = 135;
        const int Outmin = -20, Outmax = 20, MaxChange = 3;

        Queue<int> outTemp =new Queue<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0 });
        public int lastTemp = 0;
        int weatherTendency = 1;
        Random randObj = new Random((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

        public int outTempChange()
        {

            lastTemp = lastTemp + weatherTendency * randObj.Next(MaxChange);
            outTemp.Enqueue(lastTemp);
            return outTemp.Dequeue();
        }

        public void weatherTendencyChange()
        {
            if (randObj.Next(Outmax - Outmin) - 20 < lastTemp)
                weatherTendency = -1;
            else
                weatherTendency = 1;
        }

        public int calcWaterTemp()
        {
            int waterTemp = (int)( 70 - 2.5 * (outTempChange() - 6));
            if (waterTemp < 70) waterTemp = 70;
            if (waterTemp > 135) waterTemp = 135;

            return waterTemp;
        }

        public DataOut createDataOut(bool breakdown, DataIn _speed)
        {
            DataOut info = new DataOut();

            if (breakdown)
            {
                info.T_o = this.lastTemp;
                info.T_zm = 0;
                info.timestamp = _speed.symTime;
            }
            else
            {
                info.T_o = this.lastTemp;
                info.T_zm = this.calcWaterTemp();
                info.timestamp = _speed.symTime;
            }
            return info;
        }

        public void showTemp()
        {
            foreach (int i in outTemp)
            {
                Console.WriteLine("Temperatura : ", i);
            }
        }
    }
}
