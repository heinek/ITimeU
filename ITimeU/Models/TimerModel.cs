using System;
using ITimeU.DAL;

namespace ITimeU.Models
{
    public class TimerModel
    {
        public int Id { get; set; }

        private DateTime? startTime;
        public DateTime? StartTime
        {
            get
            {
                if (!IsStarted)
                    return null;
                return startTime;
            }
            private set
            {
                startTime = value;
            }
        }
        private bool isStarted = false;
        public bool IsStarted
        {
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
                var timerDal = TimerDAL.Create();
                timerDal.StartTime = startTime;
                timerDal.Save();
                Id = timerDal.TimerID;
            }
        }
    }
}
