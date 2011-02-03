
using System;
using System.Linq;
namespace ITimeU.DAL
{
    public class TimerDAL
    {
        public int TimerID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public TimerDAL()
        {

        }
        public static TimerDAL GetTimerById(int id)
        {
            using (var ctx = new ITimeUEntities())
            {
                var timer = ctx.Timers.Single(tmr => tmr.TimerID == id);
                var timerDal = new TimerDAL()
                    {
                        TimerID = timer.TimerID,
                        StartTime = timer.StartTime,
                        EndTime = timer.EndTime
                    };
                return timerDal;
            }
        }


        public static TimerDAL Create()
        {
            return new TimerDAL();
        }

        public void Save()
        {
            using (var ctx = new ITimeUEntities())
            {
                Timer timer = new Timer()
                {
                    StartTime = this.StartTime
                };
                ctx.Timers.AddObject(timer);
                ctx.SaveChanges();
                TimerID = ctx.Timers.OrderByDescending(tmr => tmr.TimerID).First().TimerID;
            }
        }
    }
}
