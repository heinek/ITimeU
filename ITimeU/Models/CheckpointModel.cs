using System.Collections.Generic;
using System.Linq;

namespace ITimeU.Models
{
    /// <summary>
    /// TODO:
    /// - Models should use the entity object properties when possible for its getters and setters!
    /// </summary>

    public class CheckpointModel
    {
        public int Id { get; set; }

        private bool dbEntryCreated
        {
            get
            {
                if (Id == 0)
                    return false;
                return true;
            }
        }

        public string Name { get; set; }

        private TimerModel timer;
        public TimerModel Timer
        {
            get
            {
                return timer;
            }

            set
            {
                timer = value;
                // Timer.SaveToDb(): The timer must exist in database so that the database foreign key
                // can be set correctly.
                timer.SaveToDb();
                SaveToDb();
            }
        }

        public int Sortorder { get; set; }
        public int? RaceId { get; set; }
        public RaceModel Race { get; private set; }

        public CheckpointModel(Checkpoint checkpoint)
        {
            Id = checkpoint.CheckpointID;
            Name = checkpoint.Name;
            if (checkpoint.RaceID.HasValue)
            {
                RaceId = checkpoint.RaceID.Value;
                Race = RaceModel.GetById(checkpoint.RaceID.Value);
            }
            if (checkpoint.TimerID.HasValue)
            {
                this.timer = TimerModel.GetTimerById(checkpoint.TimerID.Value);
                Timer = TimerModel.GetTimerById(checkpoint.TimerID.Value);
            }
            if (checkpoint.RaceID.HasValue)
                Race = RaceModel.GetById(checkpoint.RaceID.Value);

            //Race = RaceModel.GetById((int)checkpoint.RaceID);
            //var race = RaceModel.GetById(raceId);
            //int? timerid = race.GetTimerId();
            //if(timerid.HasValue)
            //Timer = TimerModel.GetTimerById(timerid.Value);
            //Sortorder = GetNextOrdernumber(raceId);
            //SaveToDb();
        }

        public CheckpointModel(string name, int raceId)
        {
            Name = name;
            Race = RaceModel.GetById(raceId);
            SaveToDb();
        }

        public CheckpointModel(string name, TimerModel timer, RaceModel race)
        {
            Name = name;
            Race = RaceModel.GetById(race.RaceId);
            Timer = timer;
            SaveToDb();
        }

        public CheckpointModel(string name, TimerModel timer, RaceModel race, int sortorder)
        {
            Name = name;
            Race = RaceModel.GetById(race.RaceId);
            RaceId = race.RaceId;
            Timer = timer;
            Sortorder = sortorder;
            SaveToDb();
        }

        public CheckpointModel()
        {
            // TODO: Complete member initialization
        }

        public void SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntry(context);
        }

        private int CreateDbEntity(Entities context)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.RaceID = RaceId;
            updateDbEntry(checkpoint);
            context.Checkpoints.AddObject(checkpoint);
            context.SaveChanges();

            return checkpoint.CheckpointID;
        }

        /// <summary>
        /// Updates a checkpoint database entity based on the properties of this model.
        /// </summary>
        /// <param name="checkpoint">The checkpoint database entity.</param>
        private void updateDbEntry(Checkpoint checkpoint)
        {
            checkpoint.Name = Name;
            checkpoint.SortOrder = Sortorder;
            if (timer != null)
                checkpoint.TimerID = timer.Id;
            if (Race != null)
                checkpoint.RaceID = Race.RaceId;
        }

        private void updateDbEntry(Entities context)
        {
            Checkpoint checkpoint = context.Checkpoints.Single(tmr => tmr.CheckpointID == Id);
            updateDbEntry(checkpoint);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets a checkpoint with the given id from the database.
        /// </summary>
        /// <param name="idToGet">The id of the checkpoint to get.</param>
        /// <returns>The retrieved checkpoint.</returns>
        public static CheckpointModel getById(int idToGet)
        {
            var entities = new Entities();
            Checkpoint checkpointDb = entities.Checkpoints.Single(temp => temp.CheckpointID == idToGet);

            return new CheckpointModel(checkpointDb);
        }

        /// <summary>
        /// Retrieves all checkpoints in database.
        /// </summary>
        /// <returns></returns>
        public static List<CheckpointModel> getAll()
        {
            using (var context = new Entities())
            {
                return context.Checkpoints.
                    Where(cp => cp.IsDeleted == false).
                    Select(cp => new CheckpointModel()
                    {
                        Id = cp.CheckpointID,
                        Name = cp.Name,
                        Sortorder = cp.SortOrder,
                        Race = new RaceModel()
                        {
                            RaceId = cp.Race.RaceID,
                            EventId = cp.Race.EventId.Value,
                            Event = new EventModel() {
                                Name = cp.Race.Event.Name
                            },
                            Name = cp.Race.Name,
                            Distance = cp.Race.Distance,
                            StartDate = cp.Race.StartDate
                        },
                        RaceId = cp.RaceID,
                    }).ToList();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CheckpointModel other = (CheckpointModel)obj;

            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode() ^
                (Timer == null ? 0.GetHashCode() : Timer.GetHashCode());
        }

        public override string ToString()
        {
            return "[Checkpoint, id: " + Id + ", name: " + Name + "]";
        }

        public static int GetNextOrdernumber(int raceid)
        {
            using (var context = new Entities())
            {
                var checkpoints = context.Checkpoints.Where(cp => cp.RaceID == raceid && !cp.IsDeleted);
                if (checkpoints.Count() > 0)
                    return checkpoints.OrderByDescending(cp => cp.SortOrder).First().SortOrder + 1;
                else
                    return 0;
            }
        }
        //public void Update()
        //{
        //    using (var context = new Entities())
        //    {
        //        var checkpoint = context.Checkpoints.Where(cp => cp.CheckpointID == Id && !cp.IsDeleted).SingleOrDefault();
        //        checkpoint.RaceID = RaceId;
        //        checkpoint.Name = Name;
        //        checkpoint.SortOrder = Sortorder;
        //        context.SaveChanges();
        //    }
        //}

        public void Delete()
        {
            using (var context = new Entities())
            {
                context.Checkpoints.Where(cp => cp.CheckpointID == Id).Single().IsDeleted = true;
                context.SaveChanges();
            }
        }

        public static List<CheckpointModel> GetCheckpoints(int raceId)
        {
            using (var context = new Entities())
            {
                return context.Checkpoints.Where(checkpoint => checkpoint.RaceID == raceId).Select(checkpoint => new CheckpointModel()
                {
                    Id = checkpoint.CheckpointID,
                    Name = checkpoint.Name,
                    RaceId = checkpoint.RaceID,
                    Sortorder = checkpoint.SortOrder
                }).ToList();
            }
        }
    }
}