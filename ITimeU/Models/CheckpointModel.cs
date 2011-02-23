using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ITimeU.Models
{
    /// <summary>
    /// TODO:
    /// - Models should use the entity object properties when possible for its getters and setters!
    /// </summary>

    public class CheckpointModel
    {
        private string p;

        public int Id { get; private set; }
        private bool dbEntryCreated = false;
        public string Name { get; private set; } // TODO: set should be private

        private TimerModel timer;
        public TimerModel Timer {
            get
            {
                return timer;
            }

            internal set
            {
                timer = value;
                // Timer.SaveToDb(): The timer must exist in database so that the database foreign key
                // can be set correctly.
                timer.SaveToDb();
                SaveToDb();
            }
        
        }

        public CheckpointModel(Checkpoint checkpoint)
        {
            Id = checkpoint.CheckpointID;
            Name = checkpoint.Name;
            if (checkpoint.Timer != null) // Is this check needed?
                Timer = new TimerModel(checkpoint.Timer);
        }

        public CheckpointModel(string checkpointName)
        {
            Name = checkpointName;
            SaveToDb();
        }

        public CheckpointModel(string checkpointName, TimerModel timer)
        {
            Name = checkpointName;
            Timer = timer;
            SaveToDb();
        }

        private void SaveToDb()
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

        private void updateDbEntry(Checkpoint checkpoint)
        {
            checkpoint.Name = Name;
            if (Timer != null)
                checkpoint.TimerID = Timer.Id;
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
            CheckpointModel other = (CheckpointModel) obj;

            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return "[Checkpoint, id: " + Id + ", name: " + Name + "]";
        }

       
    }
}