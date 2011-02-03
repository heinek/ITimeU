using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class TimerModel
    {
        private DateTime? startTime;

        public DateTime? StartTime {
            get {

                if (!IsStarted)
                {
                    return null;
                }

                return startTime;
            }
            
            private set { startTime = value; }
        }

        public TimerModel()
        {
            IsStarted = false;
        }

        public bool IsStarted { get; private set; }

        public void Start()
        {
            if (!IsStarted)
            {
                StartTime = DateTime.Now;
                IsStarted = true;
            }
        }

        
    }
}
