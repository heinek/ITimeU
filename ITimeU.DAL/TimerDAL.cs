
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
            TimerDAL timerDal = new TimerDAL();
            using (var ctx = new ITimeUEntities())
            {
                Timer timer = new Timer();
                ctx.Timers.AddObject(timer);
                ctx.SaveChanges();
                timerDal.TimerID = ctx.Timers.OrderByDescending(tmr => tmr.TimerID).First().TimerID;
            }
            return timerDal;
        }

        public void Save()
        {
            using (var ctx = new ITimeUEntities())
            {
                Timer timer = ctx.Timers.Single(tmr => tmr.TimerID == TimerID);
                timer.StartTime = this.StartTime;
                ctx.SaveChanges();
            }
        }
    }
}
