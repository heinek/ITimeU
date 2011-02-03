using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class TimerModel
    {
        private DateTime startTime;

        public DateTime StartTime { get {
            if (!IsStarted)
            {
                throw new NullReferenceException("Cannot return start time when timer hasn't started.");
            }

            return startTime;
        } private set { startTime = value; } }

        public bool IsStarted { get; private set; }

        public TimerModel()
        {
            IsStarted = false;
        }

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
