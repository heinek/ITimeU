using System.Collections.Generic;

namespace ITimeU.Models
{
    public class TimeStartnumberModel
    {
        public List<RaceIntermediateModel> Intermediates { get; set; }
        public Dictionary<int, List<RaceIntermediateModel>> CheckpointIntermediates { get; set; }
        public int CurrentCheckpointId { get; set; }
        public TimerModel Timer { get; set; }
        public CheckpointOrderModel CheckpointOrder { get; set; }

        public TimeStartnumberModel(TimerModel timer)
        {
            this.Timer = timer;
            var intermediates = new List<RaceIntermediateModel>();
            CheckpointIntermediates = new Dictionary<int, List<RaceIntermediateModel>>();
            CheckpointIntermediates.Add(timer.GetFirstCheckpointId(), intermediates);
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
            var runtimeId = Timer.AddRuntime(runtimeint, cpId).Id;
            RaceIntermediateModel raceIntermediate = new RaceIntermediateModel(cpId, checkpointOrderId, runtimeId);
            CheckpointIntermediates[CurrentCheckpointId].Add(raceIntermediate);
        }
    }
}