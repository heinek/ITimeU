using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITimeU.Models;

namespace ITimeU.Tests.Models
{
    public class RuntimeModel
    {
        public int Id { get; set; }
        public int Runtime { get; set; }
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

        public static void EditRuntime(int runtimeid, int newRuntime)
        {
            using (var ctx = new Entities())
            {
                Runtime runtimeDb = ctx.Runtimes.Single(runtimeTemp => runtimeTemp.RuntimeID == runtimeid);
                runtimeDb.Runtime1 = newRuntime;
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
    }
}
