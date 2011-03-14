using System;
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
            var checkpointOrderId =  CheckpointOrder.AddCheckpointOrderDB(cpId, startNr);
            var runtimeId = Timer.AddRuntime(runtimeint, cpId).Id;
            RaceIntermediateModel raceIntermediate = new RaceIntermediateModel(cpId, checkpointOrderId, runtimeId);
            raceIntermediate.Save();
            CheckpointIntermediates[CurrentCheckpointId].Add(raceIntermediate);
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="runtimeId">The runtimeId.</param>
        /// <param name="h">The hours.</param>
        /// <param name="m">The minutes.</param>
        /// <param name="s">The seconds.</param>
        /// <param name="ms">The millisecounds.</param>
        public void EditRuntime(int runtimeId, int h, int m, int s, int ms)
        {
            TimeSpan ts = new TimeSpan(0, h, m, s, ms);
            RuntimeModel.EditRuntime(runtimeId, (int)ts.TotalMilliseconds);
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