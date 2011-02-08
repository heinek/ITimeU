using System;
using ITimeU.DAL;

namespace ITimeU.Models
{
    [Serializable]
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
        public DateTime? EndTime { get; set; }


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

        public void Stop()
        {
            EndTime = DateTime.Now;
            var timerDal = TimerDAL.GetTimerById(Id);
            timerDal.EndTime = EndTime;
            timerDal.Save();
        }

        public void Restart()
        {
            //var newTimer = TimerDAL.Create();
            //newTimer.Save();
            Id = 0;
            IsStarted = false;
            StartTime = null;
            EndTime = null;
        }
    }
}
