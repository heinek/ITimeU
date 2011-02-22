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
        public Dictionary<int, int> RuntimeDic { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        public TimerModel()
        {
            StartTime = null;
            IsStarted = false;
            RuntimeDic = new Dictionary<int, int>();
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
        /// Saves the start time to db.
        /// </summary>
        /// <returns>TimerModel Id</returns>
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

        /// <summary>
        /// Resets this instance.
        /// </summary>
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

        // TODO: Move method to appropriate location in this class.
        /// <summary>
        /// Gets the timer by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
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
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Saves this instance.
        /// </summary>
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
        /// <summary>
        /// Adds the runtime.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        public RuntimeModel AddRuntime(int milliseconds, int checkpointid)
        {
            var newRuntime = RuntimeModel.Create(milliseconds);
            using (var ctx = new Entities())
            {
                var runtime = new Runtime() { Runtime1 = newRuntime.Runtime, CheckpointID = checkpointid };
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
                ctx.SaveChanges();
            }
        }
    }
}
