
using System.Collections.Generic;
using System.Linq;
namespace ITimeU.Models
{
    public class RaceIntermediateModel
    {
        public int CheckpointID { get; set; }
        public int CheckpointOrderID { get; set; }
        public int RuntimeId { get; set; }
        public CheckpointModel CheckpointModel { get; set; }
        public CheckpointOrderModel CheckpointorderModel { get; set; }
        public RuntimeModel RuntimeModel { get; set; }

        public RaceIntermediateModel()
        {

        }

        public RaceIntermediateModel(int checkpointId, int checkpointOrderId, int runtimeId)
        {
            CheckpointID = checkpointId;
            this.CheckpointModel = CheckpointModel.getById(checkpointId);
            CheckpointOrderID = checkpointOrderId;
            this.CheckpointorderModel = CheckpointOrderModel.GetById(checkpointOrderId);
            RuntimeId = runtimeId;
            this.RuntimeModel = RuntimeModel.getById(runtimeId);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
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
                var checkpointOrder = ctx.CheckpointOrders.Where(cpOrder => cpOrder.ID == CheckpointOrderID).Single();
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

        /// <summary>
        /// Gets the raceintermediate.
        /// </summary>
        /// <param name="cpid">The cpid.</param>
        /// <param name="cpOrderid">The cp orderid.</param>
        /// <returns></returns>
        public static RaceIntermediate GetRaceintermediate(int cpid, int cpOrderid)
        {
            using (var context = new Entities())
            {
                return context.RaceIntermediates.Where(raceintermediate => raceintermediate.CheckpointID == cpid && raceintermediate.CheckpointOrderID == cpOrderid).SingleOrDefault();
            }
        }

        /// <summary>
        /// Deletes the raceintermediate.
        /// </summary>
        /// <param name="cpid">The cpid.</param>
        /// <param name="cpOrderid">The cp orderid.</param>
        public static void DeleteRaceintermediate(int cpid, int cpOrderid)
        {
            using (var context = new Entities())
            {
                var raceintermediateToDelete = context.RaceIntermediates.Where(raceintermediate => raceintermediate.CheckpointID == cpid && raceintermediate.CheckpointOrderID == cpOrderid).SingleOrDefault();
                raceintermediateToDelete.IsDeleted = true;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the raceintermediate for race.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <returns></returns>
        public static List<RaceIntermediateModel> GetRaceintermediateForRace(int checkpointId)
        {
            using (var context = new Entities())
            {
                var list = context.RaceIntermediates.
                    Where(raceintermediate => raceintermediate.CheckpointID == checkpointId && !raceintermediate.IsDeleted).
                    Select(raceintermediate => new RaceIntermediateModel()
                    {
                        CheckpointID = raceintermediate.CheckpointID,
                        CheckpointOrderID = raceintermediate.CheckpointOrderID,
                        RuntimeId = raceintermediate.RuntimeId
                    }).ToList();
                return list;
            }
        }
    }
}
