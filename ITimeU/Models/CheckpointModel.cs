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
        public int Id { get; private set; }
        public string Name { get; set; } // TODO: set should be private
        public TimerModel Timer { get; private set; }

        // TODO: Remove this constructor if it is not used.
        private CheckpointModel(int _id, string _checkpointName)
        {
            Id = _id;
            Name = _checkpointName;
        }

        public CheckpointModel(Checkpoint checkpoint)
        {
            Id = checkpoint.CheckpointID;
            Name = checkpoint.Name;
            if (checkpoint.Timer != null)
                Timer = new TimerModel(checkpoint.Timer);
        }

        /// <summary>
        /// Gets a checkpoint with the given id from the database.
        /// </summary>
        /// <param name="idToGet">The id of the checkpoint to get.</param>
        /// <returns>The retrieved checkpoint.</returns>
        public static CheckpointModel getById(int idToGet)
        {
            return new CheckpointModel(getCheckpointDbById(idToGet));
        }

        private static Checkpoint getCheckpointDbById(int idToGet) {
            var entities = new Entities();
            return entities.Checkpoints.Single(temp => temp.CheckpointID == idToGet);
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

        /// <summary>
        /// Creates a checkpoint in the database.
        /// </summary>
        /// <param name="checkpointName">The name of the checkpoint.</param>
        /// <returns>The created checkpoint.</returns>
        public static CheckpointModel Create(string checkpointName)
        {
            Checkpoint checkpointDb = CreateDbEntity(checkpointName);
            SaveToDb(checkpointDb);
            return new CheckpointModel(checkpointDb);
        }

        private static Checkpoint CreateDbEntity(string checkpointName)
        {
            Checkpoint checkpointDb = new Checkpoint();
            checkpointDb.Name = checkpointName;

            return checkpointDb;
        }

        /// <summary>
        /// Creates a checkpoint in the database. The timings the given timer are doing, will be
        /// associated with this checkpoint.
        /// </summary>
        /// <param name="checkpointName">The name of the checkpoint.</param>
        /// <param name="timer">The timer that will do the timings for this checkpoint.</param>
        /// <returns>The created checkpoint.</returns>
        public static CheckpointModel Create(string checkpointName, TimerModel timer)
        {
            Checkpoint checkpointDb = CreateDbEntity(checkpointName, timer);
            SaveToDb(checkpointDb);
            return new CheckpointModel(checkpointDb);
        }

        private static Checkpoint CreateDbEntity(string checkpointName, TimerModel timer)
        {
            Checkpoint checkpointDb = CreateDbEntity(checkpointName);
            checkpointDb.TimerID = timer.Id;

            return checkpointDb;
        }

        private static void SaveToDb(Checkpoint checkpointDb)
        {
            var entities = new Entities();
            entities.Checkpoints.AddObject(checkpointDb);
            entities.SaveChanges();
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