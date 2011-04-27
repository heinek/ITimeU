
using System.Collections.Generic;
using System.Linq;
namespace ITimeU.Models
{
    public class RaceIntermediateModel
    {
        public int CheckpointID { get; set; }
        public CheckpointModel CheckpointModel { get; set; }
        public int CheckpointOrderID { get; set; }
        public CheckpointOrderModel CheckpointorderModel { get; set; }
        public int RuntimeId { get; set; }
        public RuntimeModel RuntimeModel { get; set; }
        public int? AthleteId { get; set; }
        public AthleteModel AthleteModel { get; set; }

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
                    RuntimeId = RuntimeId,
                    AthleteId = AthleteId
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

        public bool Update()
        {
            using (var ctx = new Entities())
            {
                var raceIntermediate = ctx.RaceIntermediates.
                    Where(raceint => raceint.CheckpointID == CheckpointID
                        && raceint.CheckpointOrderID == CheckpointOrderID
                        && raceint.RuntimeId == RuntimeId).
                        Single();
                raceIntermediate.AthleteId = AthleteId;
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
                return context.RaceIntermediates.
                    Where(raceintermediate => raceintermediate.CheckpointID == cpid && raceintermediate.CheckpointOrderID == cpOrderid).
                    SingleOrDefault();
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
                //raceintermediateToDelete.IsDeleted = true;
                RuntimeModel.DeleteRuntime(raceintermediateToDelete.RuntimeId);
                CheckpointOrderModel.DeleteCheckpointOrder(raceintermediateToDelete.CheckpointOrderID);
                context.DeleteObject(raceintermediateToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the raceintermediate for a checkpoint.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <returns></returns>
        public static List<RaceIntermediateModel> GetRaceintermediatesForCheckpoint(int checkpointId)
        {
            using (var context = new Entities())
            {
                var list = context.RaceIntermediates.
                    Where(raceintermediate => raceintermediate.CheckpointID == checkpointId && !raceintermediate.IsDeleted).
                    Select(raceintermediate => new RaceIntermediateModel()
                    {
                        CheckpointID = raceintermediate.CheckpointID,
                        CheckpointOrderID = raceintermediate.CheckpointOrderID,
                        CheckpointorderModel = new CheckpointOrderModel()
                        {
                            ID = raceintermediate.CheckpointOrderID,
                            CheckpointID = raceintermediate.CheckpointID,
                            OrderNumber = raceintermediate.CheckpointOrder.OrderNumber.HasValue ? raceintermediate.CheckpointOrder.OrderNumber.Value : 0,
                            StartingNumber = raceintermediate.CheckpointOrder.StartingNumber.HasValue ? raceintermediate.CheckpointOrder.StartingNumber.Value : 0
                        },
                        RuntimeId = raceintermediate.RuntimeId,
                        RuntimeModel = new RuntimeModel()
                        {
                            CheckPointId = raceintermediate.CheckpointID,
                            Id = raceintermediate.RuntimeId,
                            Runtime = raceintermediate.Runtime.Runtime1
                        },
                        AthleteId = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteId : null,
                        AthleteModel = new AthleteModel()
                        {
                            Id = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteId.Value : 0,
                            FirstName = raceintermediate.Athlete.FirstName,
                            LastName = raceintermediate.Athlete.LastName,
                            Club = new ClubModel()
                            {
                                Id = raceintermediate.Athlete.ClubID.HasValue ? raceintermediate.Athlete.ClubID.Value : 0,
                                Name = raceintermediate.Athlete.ClubID.HasValue ? raceintermediate.Athlete.Club.Name : " - "
                            },
                            Birthday = raceintermediate.Athlete.Birthday,
                            StartNumber = raceintermediate.Athlete.Startnumber
                        }
                    }).ToList();
                return list;
            }
        }

        /// <summary>
        /// Gets the raceintermediate for a race.
        /// </summary>
        /// <param name="raceid">The race id.</param>
        /// <returns></returns>
        public static List<RaceIntermediateModel> GetRaceintermediatesForRace(int raceid)
        {
            using (var context = new Entities())
            {
                var list = context.RaceIntermediates.
                    Where(raceintermediate => raceintermediate.Checkpoint.RaceID == raceid && !raceintermediate.IsDeleted).
                    Select(raceintermediate => new RaceIntermediateModel()
                    {
                        CheckpointID = raceintermediate.CheckpointID,
                        CheckpointModel = new CheckpointModel()
                        {
                            Id = raceintermediate.CheckpointID,
                            Name = raceintermediate.Checkpoint.Name,
                            Sortorder = raceintermediate.Checkpoint.SortOrder,
                            RaceId = raceintermediate.Checkpoint.RaceID
                        },
                        CheckpointOrderID = raceintermediate.CheckpointOrderID,
                        CheckpointorderModel = new CheckpointOrderModel()
                        {
                            ID = raceintermediate.CheckpointOrderID,
                            CheckpointID = raceintermediate.CheckpointID,
                            OrderNumber = raceintermediate.CheckpointOrder.OrderNumber.HasValue ? raceintermediate.CheckpointOrder.OrderNumber.Value : 0,
                            StartingNumber = raceintermediate.CheckpointOrder.StartingNumber.HasValue ? raceintermediate.CheckpointOrder.StartingNumber.Value : 0
                        },
                        RuntimeId = raceintermediate.RuntimeId,
                        RuntimeModel = new RuntimeModel()
                        {
                            CheckPointId = raceintermediate.CheckpointID,
                            Id = raceintermediate.RuntimeId,
                            Runtime = raceintermediate.Runtime.Runtime1
                        },
                        AthleteId = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteId : null,
                        AthleteModel = new AthleteModel()
                        {
                            Id = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteId.Value : 0,
                            FirstName = raceintermediate.AthleteId.HasValue ? raceintermediate.Athlete.FirstName : " - ",
                            LastName = raceintermediate.AthleteId.HasValue ? raceintermediate.Athlete.LastName : " - ",
                            StartNumber = raceintermediate.AthleteId.HasValue ? raceintermediate.Athlete.Startnumber : null,
                            Club = new ClubModel()
                            {
                                Id = raceintermediate.Athlete.ClubID.HasValue ? raceintermediate.Athlete.Club.ClubID : 0,
                                Name = raceintermediate.Athlete.ClubID.HasValue ? raceintermediate.Athlete.Club.Name : " - "
                            }
                        }
                    }).ToList();
                return list;
            }
        }

        public static void MergeAthletes(int raceid)
        {
            var raceintermediates = GetRaceintermediatesForRace(raceid);
            var raceathletes = RaceAthleteModel.GetAthletesConnectedToRace(raceid);
            foreach (var raceathlete in raceathletes)
            {
                foreach (var raceintermediate in raceintermediates)
                {
                    if (raceintermediate.CheckpointorderModel.StartingNumber == raceathlete.Startnumber)
                    {
                        raceintermediate.AthleteId = raceathlete.AthleteId;
                        raceintermediate.AthleteModel = AthleteModel.GetById(raceathlete.AthleteId);
                        raceintermediate.Update();
                    }
                }
            }
        }

        public void Delete()
        {
            using (var context = new Entities())
            {
                var intermediateToDelete = context.RaceIntermediates.
                    Single(intermediate => intermediate.CheckpointID == CheckpointID
                        && intermediate.CheckpointOrderID == CheckpointOrderID
                        && intermediate.RuntimeId == RuntimeId);
                context.DeleteObject(intermediateToDelete);
                context.SaveChanges();
            }
        }

        public static void DeleteRaceintermediatesForRace(int raceid)
        {
            using (var context = new Entities())
            {
                var intermediatesToDelete = context.RaceIntermediates.Where(intermediate => intermediate.Checkpoint.RaceID == raceid).ToList();
                foreach (var intermediate in intermediatesToDelete)
                {
                    context.DeleteObject(intermediate);
                }

                var runtimesToDelete = context.Runtimes.Where(runtime => runtime.Checkpoint.RaceID == raceid).ToList();
                foreach (var runtime in runtimesToDelete)
                {
                    context.DeleteObject(runtime);
                }

                var checkpointordersToDelete = context.CheckpointOrders.Where(cpo => cpo.Checkpoint.RaceID == raceid).ToList();
                foreach (var cpo in checkpointordersToDelete)
                {
                    context.DeleteObject(cpo);
                }
                context.SaveChanges();
            }
        }
    }
}
