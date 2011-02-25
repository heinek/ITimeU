using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Library;
using ITimeU.Tests.Models;

// TODO: Write class summary.

namespace ITimeU.Models
{
    [Serializable]
    public class TimerModel
    {
        private int id;
        public int Id
        {
            get
            {
                if (dbEntryCreated)
                    return id;
                else
                    return 0;
            }

            private set
            {
                id = value;
                dbEntryCreated = true;
            }

        }

        private bool dbEntryCreated = false;

        private DateTime? startTime;
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }

            /// <summary>
            /// When set, the milliseconds of the DateTime is rounded to its nearest hundred.
            /// This is done because the database has an inaccuracy (of about 3ms).
            /// </summary>
            private set
            {
                if (value == null)
                    startTime = null;
                else
                    startTime = DateTimeRounder.RoundToOneDecimal((DateTime)value);
            }

        }

        public DateTime? endTime;
        public DateTime? EndTime
        {

            get
            {
                return endTime;
            }

            private set
            {
                if (value == null)
                    endTime = null;
                else
                    endTime = DateTimeRounder.RoundToOneDecimal((DateTime)value);
            }
        }

        public bool IsStarted { get; private set; }
        public Dictionary<int, int> RuntimeDic { get; set; }
        public Dictionary<int, Dictionary<int, int>> CheckpointRuntimes { get; set; }
        public int CheckpointId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        public TimerModel()
        {
            StartTime = null;
            IsStarted = false;
            RuntimeDic = new Dictionary<int, int>();
            CheckpointRuntimes = new Dictionary<int, Dictionary<int, int>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public TimerModel(int id)
        {
            using (var context = new Entities())
            {
                var timer = context.Timers.Single(tmr => tmr.TimerID == id);
            }
            Id = id;
            StartTime = null;
            IsStarted = false;
            CheckpointId = GetFirstCheckpoint();
            RuntimeDic = new Dictionary<int, int>();
            RuntimeDic.Add(1, 2000);
            CheckpointRuntimes = new Dictionary<int, Dictionary<int, int>>();
            CheckpointRuntimes.Add(CheckpointId, RuntimeDic);
        }

        public TimerModel(Timer timer)
        {
            Id = timer.TimerID;
            StartTime = timer.StartTime;
            EndTime = timer.EndTime;

            if (StartTime != null && EndTime == null)
                IsStarted = true;
        }

        public static TimerModel GetTimerById(int id)
        {
            using (var context = new Entities())
            {
                var timer = context.Timers.Single(tmr => tmr.TimerID == id);
                return new TimerModel(timer);
            }
        }



        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                StartTime = DateTime.Now;
                EndTime = null;
                IsStarted = true;

                SaveToDb();
            }
            else
            {
                throw new InvalidOperationException("Cannot start an already started timer");
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            if (!IsStarted)
                throw new InvalidOperationException("Cannot stop a stopped timer");

            EndTime = DateTime.Now;
            IsStarted = false;

            SaveToDb();
            /// <summary>
            /// Saves the stop time stamp to db.
            /// </summary>
            /// <param name="EndTime">The end time.</param>
        }

        private void SaveToDb()
        {
            if (!dbEntryCreated)
                Id = CreateDbEntity();

            updateDbEntry();
        }

        private int CreateDbEntity()
        {
            var context = new Entities();

            Timer timer = new Timer();
            context.Timers.AddObject(timer);
            context.SaveChanges();

            dbEntryCreated = true;
            return timer.TimerID;
        }

        private void updateDbEntry()
        {
            var context = new Entities();
            Timer timer = context.Timers.Single(tmr => tmr.TimerID == Id);
            timer.StartTime = this.StartTime;
            timer.EndTime = this.EndTime;
            context.SaveChanges();
        }

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
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        public RuntimeModel AddRuntime(int milliseconds, int checkpointid)
        {
            var newRuntime = RuntimeModel.Create(milliseconds, checkpointid);
            RuntimeDic.Add(newRuntime.Id, newRuntime.Runtime);
            if (CheckpointRuntimes.ContainsKey(checkpointid))
                CheckpointRuntimes[checkpointid] = RuntimeDic;
            else
                CheckpointRuntimes.Add(checkpointid, RuntimeDic);
            if (CheckpointRuntimes.ContainsKey(checkpointid))
                CheckpointRuntimes[checkpointid] = RuntimeDic;
            else
                CheckpointRuntimes.Add(checkpointid, RuntimeDic);
            return newRuntime;
        }

        /// <summary>
        /// Adds the runtime.
        /// </summary>
        /// <param name="runtimemodel">The runtimemodel.</param>
        public void AddRuntime(RuntimeModel runtimemodel)
        {
            RuntimeModel.Create(runtimemodel.Runtime, runtimemodel.CheckPointId);
            RuntimeDic.Add(runtimemodel.Id, runtimemodel.Runtime);
            if (CheckpointRuntimes.ContainsKey(runtimemodel.CheckPointId))
                CheckpointRuntimes[runtimemodel.CheckPointId] = RuntimeDic;
            else
                CheckpointRuntimes.Add(runtimemodel.CheckPointId, RuntimeDic);
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="runtimeId">The runtime id.</param>
        /// <param name="h">The hour.</param>
        /// <param name="m">The minutes.</param>
        /// <param name="s">The seconds.</param>
        public void EditRuntime(int runtimeId, int h, int m, int s, int ms)
        {
            TimeSpan ts = new TimeSpan(0, h, m, s, ms);
            RuntimeDic[runtimeId] = (int)ts.TotalMilliseconds;
            RuntimeModel.EditRuntime(runtimeId, (int)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        public void DeleteRuntime(RuntimeModel runtime)
        {
            RuntimeDic.Remove(runtime.Id);
            CheckpointRuntimes[runtime.CheckPointId] = RuntimeDic;
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
                CheckpointRuntimes[runtimeToDelete.CheckpointID] = RuntimeDic;
                ctx.Runtimes.DeleteObject(runtimeToDelete);
            }
        }

        public override string ToString()
        {
            return "[TimerModel, id: " + Id + "]";
        }


        internal void ChangeCheckpoint(int checkpointid)
        {
            CheckpointId = checkpointid;
        }

        public int GetFirstCheckpoint()
        {
            using (var ctx = new Entities())
            {
                if (ctx.Checkpoints.Where(cp => cp.TimerID == Id).Count() == 0) return 0;
                return ctx.Checkpoints.Where(cp => cp.TimerID == Id).OrderBy(cp => cp.SortOrder).First().CheckpointID;
            }
        }

        public List<Checkpoint> GetCheckpoints()
        {
            using (var ctx = new Entities())
            {
                return ctx.Checkpoints.Where(cp => cp.TimerID == Id && cp.IsDeleted == false).OrderBy(cp => cp.SortOrder).ToList();
            }
        }
    }
}
