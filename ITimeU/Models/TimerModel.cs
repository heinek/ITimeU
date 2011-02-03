using System;

namespace ITimeU.Models
{
    public class TimerModel
    {
        //private DateTime startTime;

        //public DateTime StartTime
        //{
        //    get
        //    {
        //        if (!IsStarted)
        //        {
        //            throw new NullReferenceException("Cannot return start time when timer hasn't started.");
        //        }

        //        return startTime;
        //    }
        //    private set { startTime = value; }
        //}
        public DateTime StartTime { get; set; }
        private bool isStarted = false;
        public bool IsStarted {
            get
            {
                return isStarted;
            }
            private set
            {
                isStarted = value;
            }
        }

        public TimerModel()
        {
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
