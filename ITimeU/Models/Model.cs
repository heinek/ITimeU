using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace ITimeU.Models
{
    public abstract class Model<E> : Instantiatable<E>
        where E : EntityObject
    {
        private static Entities entitiesStatic = new Entities();
        private const int ID_WHEN_NOT_CREATED_IN_DB = -1;

        /// <summary>
        /// The identifier for this model. Equals to the primary key in the database,
        /// </summary>
        public int Id { get; protected set; } // TODO: Set nullable?

        private bool instanceSaved
        {
            get
            {
                if (Id == ID_WHEN_NOT_CREATED_IN_DB)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Creates an empty model.
        /// WARNING: Should normally NOT be used by external callers.
        /// </summary>
        public Model()
        {
            SetDefaultId();
        }

        /// <summary>
        /// Instantiates this model based on a database entity.
        /// </summary>
        /// <param name="dbEntity">The database entity to read fields from</param>
        public abstract void InstantiateFrom(E dbEntity);

        /// <summary>
        /// Deletes this model from database.
        /// </summary>
        public void DeleteFromDb()
        {
            var entities = new Entities();

            ObjectSet<E> dbObjectSet = GetModelObjectSetFrom(entities);
            E entityToDelete = GetMeIn(dbObjectSet);
            dbObjectSet.DeleteObject(entityToDelete);

            entities.SaveChanges();

            SetDefaultId();
        }

        protected abstract ObjectSet<E> GetModelObjectSetFrom(Entities entities);
        
        protected abstract E GetMeIn(ObjectSet<E> dbObjectSet);

        private void SetDefaultId()
        {
            Id = ID_WHEN_NOT_CREATED_IN_DB;
        }

        internal int SaveToDb()
        {
            if (!instanceSaved)
                Id = GetOrCreateDbEntity();
            else
                updateDbEntity();

            return Id;
        }

        protected abstract int GetOrCreateDbEntity();

        protected abstract void updateDbEntity();

    }
}