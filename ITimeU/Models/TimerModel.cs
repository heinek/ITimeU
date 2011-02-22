using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Tests.Models;

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
        //public List<RuntimeModel> Runtimes { get; set; }
        public Dictionary<int, int> RuntimeDic { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        public TimerModel()
        {
            StartTime = null;
            IsStarted = false;
            //Runtimes = new List<RuntimeModel>();
            //Runtimes.Add(new RuntimeModel());
            RuntimeDic = new Dictionary<int, int>();
        }

        public TimerModel(Timer timer)
        {
            StartTime = timer.StartTime;
            EndTime = timer.EndTime;
            Id = timer.TimerID;
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

        /// <summary>
        /// Sets the start timestamp.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        private void SetStartTimestamp(DateTime startTime)
        {
            StartTime = startTime;
            IsStarted = true;
        }

        /// <summary>
        /// </summary>
        /// <returns>The ID of the new timer generated.</returns>
        private int SaveStartTimeToDb()
        {
            var context = new Entities();

            Timer timer = new Timer();
            timer.StartTime = StartTime;
            context.Timers.AddObject(timer);
            context.SaveChanges();

            return timer.TimerID;
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

        /// <summary>
        /// Saves the stop time stamp to db.
        /// </summary>
        /// <param name="EndTime">The end time.</param>
        private void SaveStopTimeStampToDb(DateTime? EndTime)
        {
            var timer = GetTimerById(Id);
            timer.EndTime = EndTime;
            timer.Save();
        }

        public void Save()
        {
            using (var context = new Entities())
            {
                Timer timer = context.Timers.Single(tmr => tmr.TimerID == Id);

                timer.StartTime = this.StartTime;
                if (this.EndTime.HasValue)
                    timer.EndTime = this.EndTime;

                context.SaveChanges();
            }
        }

        public void Reset()
        {
            if (IsStarted)
                throw new InvalidOperationException(
                    "Cannot reset a started timer. Stop timer before resetting.");

            IsStarted = false;
            StartTime = null;
            EndTime = null;
        }

        /// <summary>
        /// Gets the timer by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static TimerModel Create()
        {
            TimerModel timerModel = new TimerModel();

            using (var context = new Entities())
            {
                Timer timer = new Timer();
                context.Timers.AddObject(timer);
                context.SaveChanges();
                timerModel.Id = context.Timers.OrderByDescending(tmr => tmr.TimerID).First().TimerID;
            }

            return timerModel;
        }
        
        /// <summary>
        /// Saves this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            TimerModel other = (TimerModel)obj;

            return
                Id == other.Id &&
                IsStarted == other.IsStarted &&
                StartTime == other.StartTime &&
                EndTime == other.EndTime;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ IsStarted.GetHashCode()
                ^ (StartTime == null ? DateTime.MinValue.GetHashCode() : StartTime.GetHashCode())
                ^ (EndTime == null ? DateTime.MinValue.GetHashCode() : EndTime.GetHashCode());
        }

        /// <summary>
        /// Adds the runtime.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns></returns>
        public RuntimeModel AddRuntime(int milliseconds)
        {
            var newRuntime = RuntimeModel.Create(milliseconds);
            //Runtimes.Add(newRuntime);
            using (var ctx = new Entities())
            {
                var runtime = new Runtime() { Runtime1 = newRuntime.Runtime };
                ctx.Runtimes.AddObject(runtime);
                ctx.SaveChanges();
                newRuntime.Id = runtime.RuntimeID;
                RuntimeDic.Add(runtime.RuntimeID, runtime.Runtime1);
            }
            return newRuntime;
        }

        /// <summary>
        /// Adds the runtime.
        /// </summary>
        /// <param name="runtimemodel">The runtimemodel.</param>
        public void AddRuntime(RuntimeModel runtimemodel)
        {
            var runtime = new Runtime() { Runtime1 = runtimemodel.Runtime };
            using (var ctx = new Entities())
            {
                ctx.Runtimes.AddObject(runtime);
                ctx.SaveChanges();
                runtimemodel.Id = runtime.RuntimeID;
            }
            //Runtimes.Add(runtimemodel);
            RuntimeDic.Add(runtime.RuntimeID, runtime.Runtime1);

        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="runtimeId">The runtime id.</param>
        /// <param name="newRuntime">The new runtime.</param>
        public void EditRuntime(int runtimeId, int newRuntime)
        {
            RuntimeDic[runtimeId] = newRuntime;
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        public void DeleteRuntime(RuntimeModel runtime)
        {
            RuntimeDic.Remove(runtime.Id);
            using (var ctx = new Entities())
            {
                var runtimeToDelete = ctx.Runtimes.Where(runt => runt.RuntimeID == runtime.Id).Single();
                ctx.Runtimes.DeleteObject(runtimeToDelete);
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public void DeleteRuntime(int runtimeid)
        {
            RuntimeDic.Remove(runtimeid);
            using (var ctx = new Entities())
            {
                var runtimeToDelete = ctx.Runtimes.Where(runt => runt.RuntimeID == runtimeid).Single();
                ctx.Runtimes.DeleteObject(runtimeToDelete);
            }
        }

        public override string ToString()
        {
            return "[TimerModel, id: " + Id + "]";
        }
    }
}
