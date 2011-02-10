using System;
using System.Linq;
using ITimeU.Logging;

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
            return Create().Id;
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
            var timer = GetTimerById(Id);
            timer.EndTime = EndTime;
            timer.Save();
        }

        public void Reset()
        {
            if (IsStarted)
                throw new InvalidOperationException(
                    "Cannot reset a started timer. Stop timer before resetting.");

            Id = 0;
            IsStarted = false;
            StartTime = null;
            EndTime = null;
        }

        public static TimerModel GetTimerById(int id)
        {
            using (var ctx = new Entities())
            {
                var timer = ctx.Timers.Single(tmr => tmr.TimerID == id);
                var timerDal = new TimerModel()
                {
                    Id = timer.TimerID,
                    StartTime = timer.StartTime,
                    EndTime = timer.EndTime
                };
                return timerDal;
            }
        }

        public static TimerModel Create()
        {
            TimerModel timerModel = new TimerModel();

            using (var ctx = new Entities())
            {
                Timer timer = new Timer();
                ctx.Timers.AddObject(timer);
                ctx.SaveChanges();
                timerModel.Id = ctx.Timers.OrderByDescending(tmr => tmr.TimerID).First().TimerID;
            }

            return timerModel;
        }

        public void Save()
        {
            using (var ctx = new Entities())
            {
                Timer timer = ctx.Timers.Single(tmr => tmr.TimerID == Id);

                timer.StartTime = this.StartTime;
                if (this.EndTime.HasValue)
                    timer.EndTime = this.EndTime;

                ctx.SaveChanges();
            }
        }
    }
}
