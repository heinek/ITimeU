using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class TimerModel
    {
        private DateTime startTime;
        public DateTime StartTime { get { return startTime; } private set { startTime = value; } }


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
