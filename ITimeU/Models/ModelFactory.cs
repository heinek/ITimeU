using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

namespace ITimeU.Models
{
    class ModelFactory<E, M>
        where E : EntityObject, new()
        where M : Model<E>, new()
    {
        private Entities entitiesStatic;
        private ObjectSet<E> objectSet;

        public ModelFactory(Entities entitiesStatic, ObjectSet<E> objectSet)
        {
            this.entitiesStatic = entitiesStatic;
            this.objectSet = objectSet;
        }

        /// <summary>
        /// Creates models for all for each database entity in the given object set.
        /// </summary>
        /// <returns>A list containing all the models.s</returns>
        public List<M> GetAll()
        {
            IEnumerable<E> dbEntities = objectSet.AsEnumerable<E>();

            List<M> models = new List<M>();
            foreach (E dbEntity in dbEntities)
            {
                M model = new M();
                model.InstantiateFrom(dbEntity);
                models.Add(model);
            }

            return models;
        }

        internal void SaveEntityToDb(E dbEntity)
        {
            objectSet.AddObject(dbEntity);
            entitiesStatic.SaveChanges();
        }
    }
}
