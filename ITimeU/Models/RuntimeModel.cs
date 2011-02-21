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

        public RuntimeModel()
        {

        }
        private RuntimeModel(int _id, int _runtime)
        {
            Id = _id;
            Runtime = _runtime;
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
                return new RuntimeModel(runtimeDb.RuntimeID, runtimeDb.Runtime1);
            }
        }

        /// <summary>
        /// Creates a runtime in the database.
        /// </summary>
        /// <param name="runtime">The runtime in milliseconds.</param>
        /// <returns>The created runtime.</returns>
        public static RuntimeModel Create(int runtime)
        {
            Runtime runtimeDb = CreateDbEntity(runtime);
            SaveToDb(runtimeDb);

            return new RuntimeModel(runtimeDb.RuntimeID, runtimeDb.Runtime1);
        }

        private static Runtime CreateDbEntity(int runtime)
        {
            Runtime runtimeDb = new Runtime();
            runtimeDb.Runtime1 = runtime;

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
