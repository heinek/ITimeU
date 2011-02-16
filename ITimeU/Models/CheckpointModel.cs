using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class CheckpointModel
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        private CheckpointModel(int _id, string _checkpointName)
        {
            Id = _id;
            Name = _checkpointName;
        }

        /// <summary>
        /// Gets a checkpoint with the given id from the database.
        /// </summary>
        /// <param name="idToGet">The id of the checkpoint to get.</param>
        /// <returns>The retrieved checkpoint.</returns>
        public static CheckpointModel getById(int idToGet)
        {
            var ctx = new Entities();
            Checkpoint checkpointDb = ctx.Checkpoints.Single(temp => temp.CheckpointID == idToGet);
            return new CheckpointModel(checkpointDb.CheckpointID, checkpointDb.Name);
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
            return new CheckpointModel(checkpointDb.CheckpointID, checkpointDb.Name);
        }

        private static Checkpoint CreateDbEntity(string checkpointName)
        {
            Checkpoint checkpointDb = new Checkpoint();
            checkpointDb.Name = checkpointName;

            return checkpointDb;
        }

        private static void SaveToDb(Checkpoint checkpointDb)
        {
            var ctx = new Entities();
            ctx.Checkpoints.AddObject(checkpointDb);
            ctx.SaveChanges();
        }

        // override object.Equals
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