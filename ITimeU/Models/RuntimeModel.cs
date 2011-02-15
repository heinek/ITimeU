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

        private RuntimeModel(int _id, int _runtime)
        {
            Id = _id;
            Runtime = _runtime;
        }

        public static RuntimeModel getById(int runtimeId)
        {
            using (var ctx = new Entities())
            {
                Runtime runtimeDb = ctx.Runtimes.Single(runtimeTemp => runtimeTemp.RuntimeID == runtimeId);
                return new RuntimeModel(runtimeDb.RuntimeID, runtimeDb.Runtime1);
            }
        }

        public static RuntimeModel Create(int runtime)
        {
            Runtime runtimeDb = CreateDbEntity(runtime);
            SaveToDb(runtimeDb);

            return new RuntimeModel(runtimeDb.RuntimeID, runtime);
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
