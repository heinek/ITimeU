using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;

namespace ITimeU.Models
{
    public class RuntimeModel
    {
        public int Id { get; set; }
        public int Runtime { get; set; }
        public string RuntimeToTime { 
            get
            {
                return Runtime.ToTimerString();
            }
        }
        public int CheckPointId { get; set; }

        public RuntimeModel()
        {

        }
        private RuntimeModel(int _id, int _runtime, int _checkpointId)
        {
            Id = _id;
            Runtime = _runtime;
            CheckPointId = _checkpointId;
        }

        /// <summary>
        /// Gets a runtime with the given id from the database.
        /// </summary>
        /// <param name="idToGet">The id of the runtime to get.</param>
        /// <returns>The retrieved runtime.</returns>
        public static RuntimeModel getById(int runtimeId)
        {
            using (var ctx = new Entities())
            {
                Runtime runtimeDb = ctx.Runtimes.Single(runtimeTemp => runtimeTemp.RuntimeID == runtimeId);
                return new RuntimeModel(runtimeDb.RuntimeID, runtimeDb.Runtime1, runtimeDb.CheckpointID);
            }
        }

        /// <summary>
        /// Creates a runtime in the database.
        /// </summary>
        /// <param name="runtime">The runtime in milliseconds.</param>
        /// <returns>The created runtime.</returns>
        public static RuntimeModel Create(int runtime, int checkpointId)
        {
            Runtime runtimeDb = CreateDbEntity(runtime, checkpointId);
            SaveToDb(runtimeDb);

            return new RuntimeModel(runtimeDb.RuntimeID, runtimeDb.Runtime1, checkpointId);
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public static void DeleteRuntime(int runtimeid)
        {
            using (var ctx = new Entities())
            {
                var runtimeToDelete = ctx.Runtimes.Where(runt => runt.RuntimeID == runtimeid).Single();
                ctx.Runtimes.DeleteObject(runtimeToDelete);
                ctx.SaveChanges();
            }
        }

        public static void EditRuntime(int runtimeid, int newRuntime)
        {
            using (var ctx = new Entities())
            {
                Runtime runtimeDb = ctx.Runtimes.Single(runtimeTemp => runtimeTemp.RuntimeID == runtimeid);
                runtimeDb.Runtime1 = newRuntime;
                ctx.SaveChanges();
            }
        }

        public static void EditRuntime(int runtimeid, int h, int m, int s, int ms)
        {
            TimeSpan ts = new TimeSpan(0, h, m, s, ms);
            using (var ctx = new Entities())
            {
                Runtime runtimeDb = ctx.Runtimes.Single(runtimeTemp => runtimeTemp.RuntimeID == runtimeid);
                runtimeDb.Runtime1 = Convert.ToInt32(ts.TotalMilliseconds);
                ctx.SaveChanges();
            }
        }

        private static Runtime CreateDbEntity(int runtime, int checkpointId)
        {
            Runtime runtimeDb = new Runtime();
            runtimeDb.Runtime1 = runtime;
            runtimeDb.CheckpointID = checkpointId;
            return runtimeDb;
        }

        private static void SaveToDb(Runtime runtimeDb)
        {
            var ctx = new Entities();
            ctx.Runtimes.AddObject(runtimeDb);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Gets the runtimes.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <returns></returns>
        public static Dictionary<int, int> GetRuntimes(int checkpointId)
        {
            using (var context = new Entities())
            {
                return context.Runtimes.Where(runtime => runtime.CheckpointID == checkpointId && runtime.IsMerged == false).ToDictionary(runtime => runtime.RuntimeID, runtime => runtime.Runtime1);
            }
        }
    }
}
