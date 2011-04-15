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
        public int Id { get; private set; }

        private bool dbEntryCreated
        {
            get
            {
                if (Id == 0)
                    return false;
                return true;
            }
        }

        public string Name { get; private set; }

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
        public RaceModel Race { get; private set; }

        public CheckpointModel(Checkpoint checkpoint)
        {
            Id = checkpoint.CheckpointID;
            Name = checkpoint.Name;
            if (checkpoint.TimerID != null)
                this.timer = TimerModel.GetTimerById((int)checkpoint.TimerID);

            Race = RaceModel.GetById((int)checkpoint.RaceID);
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
            Timer = timer; 
            Sortorder = sortorder;
            SaveToDb();
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
            var entities = new Entities();
            IEnumerable<Checkpoint> checkpoints = entities.Checkpoints.AsEnumerable<Checkpoint>();

            List<CheckpointModel> models = new List<CheckpointModel>();
            foreach (Checkpoint checkpoint in checkpoints)
            {
                CheckpointModel converted = new CheckpointModel(checkpoint);
                models.Add(converted);
            }

            return models;
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
    }
}