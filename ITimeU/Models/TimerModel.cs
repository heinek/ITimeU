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
        public int? RaceID { get; set; }
        public bool IsStarted { get; private set; }
        public Dictionary<int, Dictionary<int, int>> CheckpointRuntimes { get; set; }
        public int CurrentCheckpointId { get; set; }
        private bool dbEntryCreated = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        public TimerModel()
        {
            StartTime = null;
            IsStarted = false;
            CheckpointRuntimes = new Dictionary<int, Dictionary<int, int>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public TimerModel(int id)
        {
            Id = id;
            StartTime = null;
            IsStarted = false;
            CurrentCheckpointId = GetFirstCheckpointId();
            CheckpointRuntimes = new Dictionary<int, Dictionary<int, int>>();
            CheckpointRuntimes.Add(CurrentCheckpointId, new Dictionary<int, int>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerModel"/> class.
        /// </summary>
        /// <param name="timer">The timer.</param>
        public TimerModel(Timer timer)
        {
            Id = timer.TimerID;
            StartTime = timer.StartTime;
            EndTime = timer.EndTime;

            if (StartTime != null && EndTime == null)
                IsStarted = true;
        }

        /// <summary>
        /// Gets the timer by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
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
        }

        /// <summary>
        /// Saves the stop time stamp to db.
        /// </summary>
        /// <param name="EndTime">The end time.</param>
        public void SaveToDb()
        {
            if (!dbEntryCreated)
                Id = CreateDbEntity();

            updateDbEntry();
        }

        /// <summary>
        /// Creates the db entity.
        /// </summary>
        /// <returns></returns>
        private int CreateDbEntity()
        {
            var context = new Entities();

            Timer timer = new Timer();
            context.Timers.AddObject(timer);
            context.SaveChanges();

            dbEntryCreated = true;
            return timer.TimerID;
        }

        /// <summary>
        /// Updates the db entry.
        /// </summary>
        private void updateDbEntry()
        {
            var context = new Entities();
            Timer timer = context.Timers.Single(tmr => tmr.TimerID == Id);
            timer.StartTime = this.StartTime;
            timer.EndTime = this.EndTime;
            if (RaceID.HasValue) timer.RaceID = this.RaceID.Value;
            context.SaveChanges();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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
            var checkpointRuntimeDic = CheckpointRuntimes[checkpointid];
            checkpointRuntimeDic.Add(newRuntime.Id, newRuntime.Runtime);
            if (CheckpointRuntimes.ContainsKey(checkpointid))
                CheckpointRuntimes[checkpointid] = checkpointRuntimeDic;
            else
                CheckpointRuntimes.Add(checkpointid, checkpointRuntimeDic);
            return newRuntime;
        }

        /// <summary>
        /// Adds the runtime.
        /// </summary>
        /// <param name="runtimemodel">The runtimemodel.</param>
        public void AddRuntime(RuntimeModel runtimemodel)
        {
            RuntimeModel.Create(runtimemodel.Runtime, runtimemodel.CheckPointId);
            var checkpointRuntimeDic = CheckpointRuntimes[CurrentCheckpointId];
            checkpointRuntimeDic.Add(runtimemodel.Id, runtimemodel.Runtime);
            if (CheckpointRuntimes.ContainsKey(CurrentCheckpointId))
                CheckpointRuntimes[CurrentCheckpointId] = checkpointRuntimeDic;
            else
                CheckpointRuntimes.Add(CurrentCheckpointId, checkpointRuntimeDic);
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
            CheckpointRuntimes[CurrentCheckpointId][runtimeId] = (int)ts.TotalMilliseconds;
            RuntimeModel.EditRuntime(runtimeId, (int)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        public void DeleteRuntime(RuntimeModel runtime)
        {
            CheckpointRuntimes[CurrentCheckpointId].Remove(runtime.Id);
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
            CheckpointRuntimes[CurrentCheckpointId].Remove(runtimeid);
            using (var ctx = new Entities())
            {
                var runtimeToDelete = ctx.Runtimes.Where(runt => runt.RuntimeID == runtimeid).Single();
                ctx.Runtimes.DeleteObject(runtimeToDelete);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "[TimerModel, id: " + Id + "]";
        }

        /// <summary>
        /// Changes the checkpoint.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        public void ChangeCheckpoint(int checkpointid)
        {
            CurrentCheckpointId = checkpointid;
            if (!CheckpointRuntimes.ContainsKey(checkpointid))
            {
                CheckpointRuntimes.Add(checkpointid, new Dictionary<int, int>());
            }
        }

        /// <summary>
        /// Gets the first checkpoint.
        /// </summary>
        /// <returns></returns>
        public int GetFirstCheckpointId()
        {
            using (var ctx = new Entities())
            {
                if (ctx.Checkpoints.Where(cp => cp.TimerID == Id).Count() == 0) return 0;
                return ctx.Checkpoints.Where(cp => cp.TimerID == Id).OrderBy(cp => cp.SortOrder).First().CheckpointID;
            }
        }

        /// <summary>
        /// Gets all the checkpoints for .
        /// </summary>
        /// <returns></returns>
        public List<Checkpoint> GetCheckpoints()
        {
            using (var ctx = new Entities())
            {
                return ctx.Checkpoints.Where(cp => cp.TimerID == Id && cp.IsDeleted == false).OrderBy(cp => cp.SortOrder).ToList();
            }
        }

        public static List<Timer> GetTimers(int raceId)
        {
            using (var ctx = new Entities())
            {
                return ctx.Timers.Where(timer => timer.IsDeleted == false && timer.RaceID == raceId).ToList();
            }
        }
    }
}
