
using System.Collections.Generic;
using System.Linq;

namespace ITimeU.Models
{
    public class TimeMergerModel
    {
        public int CheckpointID { get; set; }
        public int CheckpointOrderID { get; set; }
        public int RuntimeId { get; set; }
        public List<Timer> Timers { get; set; }

        public TimeMergerModel()
        {
            Timers = new List<Timer>();
        }

        public static void MergeTimestamp(int checkpointid, int runtimeid)
        {
            using (var context = new Entities())
            {
                var availableCheckpointorders = context.CheckpointOrders.Where(cpo => !cpo.IsDeleted && !cpo.IsMerged).OrderBy(cpo => cpo.OrderNumber).ToList();
                if (availableCheckpointorders.Count == 0) return;
                Merge(checkpointid, availableCheckpointorders.First().ID, runtimeid);

            }
        }

        public static void Merge(int checkpointId)
        {
            using (var context = new Entities())
            {
                var timestamps = context.Runtimes.Where(runtime => runtime.CheckpointID == checkpointId && !runtime.IsDeleted).OrderBy(runtime => runtime.Runtime1).ToList();
                var startnumbers = context.CheckpointOrders.Where(startnumber => startnumber.CheckpointID == checkpointId && !startnumber.IsDeleted).OrderBy(startnumber => startnumber.OrderNumber).ToList();
                var raceintermediates = context.RaceIntermediates.Where(intermediate => intermediate.CheckpointID == checkpointId).ToList();
                //Removes exicting entries in the database
                foreach (var intermediate in raceintermediates)
                {
                    context.DeleteObject(intermediate);
                }
                context.SaveChanges();
                //Creates new entries
                int i = 0;
                foreach (var timestamp in timestamps)
                {
                    if (startnumbers.Count < i + 1)
                        break;
                    Merge(checkpointId, startnumbers[i].ID, timestamp.RuntimeID);
                    i++;
                }
            }
            var checkpoint = CheckpointModel.getById(checkpointId);
            RaceIntermediateModel.MergeAthletes(checkpoint.RaceId.Value);
        }

        public static Stack<int> Merge(int checkpointId, string timestampdata, string startnumberdata)
        {
            var timestamps = timestampdata.Split(',');
            var timestampstack = new Stack<int>();
            foreach (var timestamp in timestamps)
            {
                timestampstack.Push(int.Parse(timestamp));
            }
            return timestampstack;
        }

        public static void Merge(int checkpointId, Dictionary<int, int> dicTimestamps, List<CheckpointOrder> startnumbers)
        {
            int i = 0;
            foreach (var kvp in dicTimestamps.OrderBy(timestamp => timestamp.Value))
            {
                if (startnumbers.Count < i + 1)
                    break;
                Merge(checkpointId, startnumbers.OrderBy(stnumb => stnumb.OrderNumber).ToList()[i].ID, kvp.Key);
                i++;
            }
        }

        public static RaceIntermediateModel Merge(int checkpointId, int checkpointOrderId, int runtimeId)
        {
            var raceIntermediateModel = new RaceIntermediateModel(checkpointId, checkpointOrderId, runtimeId);
            if (raceIntermediateModel.Save())
            {
                var cpo = CheckpointOrderModel.GetById(checkpointOrderId);
                cpo.IsMerged = true;
                cpo.Update();

                var runtime = RuntimeModel.getById(runtimeId);
                runtime.IsMerged = true;
                runtime.Update();

                return raceIntermediateModel;
            }
            return null;
        }

        public static List<RaceIntermediate> GetMergedList(int checkpointId)
        {
            using (var context = new Entities())
            {
                return context.RaceIntermediates.Where(intermediates => intermediates.IsDeleted == false && intermediates.CheckpointID == checkpointId).ToList();
            }
        }
    }
}