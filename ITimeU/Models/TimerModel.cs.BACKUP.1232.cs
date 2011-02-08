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
<<<<<<< HEAD
            StartTime = null;
            IsStarted = false;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
=======
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
>>>>>>> master
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
<<<<<<< HEAD

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

=======

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

>>>>>>> master
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
