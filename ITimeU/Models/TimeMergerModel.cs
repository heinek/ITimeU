
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
                Merge(checkpointId, startnumbers[i].ID, kvp.Key);
                i++;
            }
        }

        public static RaceIntermediateModel Merge(int checkpointId, int checkpointOrderId, int runtimeId)
        {
            var raceIntermediateModel = new RaceIntermediateModel(checkpointId, checkpointOrderId, runtimeId);
            if (raceIntermediateModel.Save()) return raceIntermediateModel;
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