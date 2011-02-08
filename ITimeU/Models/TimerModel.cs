using System;
using ITimeU.DAL;

// TODO: Write class summary.

namespace ITimeU.Models
{
    [Serializable]
    public class TimerModel
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public bool IsStarted { get; private set; }

        public TimerModel()
        {
            StartTime = null;
            IsStarted = false;
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
            else
            {
                throw new InvalidOperationException("Cannot start an already started timer");
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
            timerDal.StartTime = StartTime;
            timerDal.Save();

            return timerDal.TimerID;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            IsStarted = false;
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
