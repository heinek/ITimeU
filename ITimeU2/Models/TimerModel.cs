using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITimeU2
{
    public class TimerModel
    {
        
        public DateTime StartTime { get; private set; }

        public TimerModel()
        {
        }

        public void Start()
        {
            if (StartTime == null)
            {
                StartTime = DateTime.Now;
            }
         }

    }
}
