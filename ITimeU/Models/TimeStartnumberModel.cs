using System.Collections.Generic;
using System.Linq;

namespace ITimeU.Models
{
    public class TimeStartnumberModel
    {
        public List<RaceIntermediateModel> Intermediates { get; set; }
        public Dictionary<int, List<RaceIntermediateModel>> CheckpointIntermediates { get; set; }
        public int CurrentCheckpointId { get; private set; }
        public TimerModel Timer { get; set; }
        public CheckpointOrderModel CheckpointOrder { get; set; }

        public TimeStartnumberModel(TimerModel timer)
        {
            this.Timer = timer;
            var intermediates = new List<RaceIntermediateModel>();
            CheckpointIntermediates = new Dictionary<int, List<RaceIntermediateModel>>();
            CheckpointIntermediates.Add(timer.GetFirstCheckpointId(), intermediates);
            using (var context = new Entities())
            {
                foreach (var checkpoint in CheckpointModel.GetCheckpoints(timer.RaceID.Value))
                {
                    var raceintermediates = context.RaceIntermediates.Where(interm => interm.CheckpointID == checkpoint.Id).Select(interm => new RaceIntermediateModel()
                    {
                        AthleteId = interm.AthleteId,
                        CheckpointID = interm.CheckpointID,
                        CheckpointOrderID = interm.CheckpointOrderID,
                        RuntimeId = interm.RuntimeId
                    }).ToList();
                    if (!CheckpointIntermediates.ContainsKey(checkpoint.Id))
                        CheckpointIntermediates.Add(checkpoint.Id, raceintermediates);
                    else
                        CheckpointIntermediates[checkpoint.Id] = raceintermediates;
                }
            }
            this.Intermediates = intermediates;
        }

        /// <summary>
        /// Changes the checkpoint.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        public void ChangeCheckpoint(int checkpointid)
        {
            CurrentCheckpointId = checkpointid;
            if (!CheckpointIntermediates.ContainsKey(checkpointid))
            {
                CheckpointIntermediates.Add(checkpointid, new List<RaceIntermediateModel>());
            }
        }

        /// <summary>
        /// Adds the startnumber.
        /// </summary>
        /// <param name="cpId">The cp id.</param>
        /// <param name="startNr">The start nr.</param>
        /// <param name="runtimeint">The runtimeint.</param>
        /// <returns></returns>
        public void AddStartnumber(int cpId, int startNr, int runtimeint)
        {
            var checkpointOrderId = CheckpointOrder.AddCheckpointOrderDB(cpId, startNr);
            if (checkpointOrderId == 0) return;
            var runtimeId = Timer.AddRuntime(runtimeint, cpId).Id;
            RaceIntermediateModel raceIntermediate = new RaceIntermediateModel(cpId, checkpointOrderId, runtimeId);
            raceIntermediate.Save();
            CheckpointIntermediates[CurrentCheckpointId].Add(raceIntermediate);
        }

        /// <summary>
        /// Deletes the raceintermediate.
        /// </summary>
        /// <param name="cpid">The cpid.</param>
        /// <param name="cporderid">The cporderid.</param>
        public void DeleteRaceintermediate(int cpid, int cporderid)
        {

            CheckpointIntermediates[CurrentCheckpointId].Remove(CheckpointIntermediates[CurrentCheckpointId].Where(raceintermediate => raceintermediate.CheckpointID == cpid && raceintermediate.CheckpointOrderID == cporderid).Single());
            RaceIntermediateModel.DeleteRaceintermediate(cpid, cporderid);
        }

        /// <summary>
        /// Edits the startnumber.
        /// </summary>
        /// <param name="cpid">The cpid.</param>
        /// <param name="cporderid">The cporderid.</param>
        /// <param name="newstartnumber">The newstartnumber.</param>
        public void EditStartnumber(int cpid, int cporderid, int newstartnumber)
        {
            CheckpointOrderModel.EditCheckpointOrder(cporderid, newstartnumber);
        }
    }
}