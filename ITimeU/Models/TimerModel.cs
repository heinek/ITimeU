using System;
using ITimeU.DAL;

// TODO: Write class summary.

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

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                SetStartTimestamp(DateTime.Now);
                Id = SaveStartTimeToDb();
            }
        }

        private void SetStartTimestamp(DateTime startTime)
        {
            StartTime = startTime;
            IsStarted = true;
        }

        private int SaveStartTimeToDb()
        {
            var timerDal = TimerDAL.Create();
            timerDal.StartTime = startTime;
            timerDal.Save();

            return timerDal.TimerID;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            EndTime = DateTime.Now;
            SaveStopTimeStampToDb(EndTime);
        }

        private void SaveStopTimeStampToDb(DateTime? EndTime)
        {
            var timerDal = TimerDAL.GetTimerById(Id);
            timerDal.EndTime = EndTime;
            timerDal.Save();
        }
    }
}
