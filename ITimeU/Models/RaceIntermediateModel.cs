
using System.Linq;
namespace ITimeU.Models
{
    public class RaceIntermediateModel
    {
        public int CheckpointID { get; set; }
        public int CheckpointOrderID { get; set; }
        public int RuntimeId { get; set; }

        public RaceIntermediateModel()
        {

        }

        public RaceIntermediateModel(int checkpointId, int checkpointOrderId, int runtimeId)
        {
            CheckpointID = checkpointId;
            CheckpointOrderID = checkpointOrderId;
            RuntimeId = runtimeId;
        }

        public bool Save()
        {
            using (var ctx = new Entities())
            {
                var raceIntermediate = new RaceIntermediate()
                {
                    CheckpointID = CheckpointID,
                    CheckpointOrderID = CheckpointOrderID,
                    RuntimeId = RuntimeId
                };
                ctx.RaceIntermediates.AddObject(raceIntermediate);
                var checkpointOrder = ctx.CheckpointOrders.Where(cpOrder => cpOrder.ID == CheckpointID).Single();
                checkpointOrder.IsMerged = true;
                var runtime = ctx.Runtimes.Where(rt => rt.RuntimeID == RuntimeId).Single();
                runtime.IsMerged = true;
                try
                {
                    ctx.SaveChanges();
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
