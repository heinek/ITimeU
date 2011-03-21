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
        private static int ID_WHEN_NOT_CREATED_IN_DB = -1;

        // TODO: Comment.
        public int Id { get; protected set; } // TODO: Set nullable?

        // TODO: Comment.
        public static List<M> GetAll<M>(ObjectSet<E> databaseSet)
            where M : Model<E>, new()
        {
            IEnumerable<E> dbEntities = databaseSet.AsEnumerable<E>();

            List<M> models = new List<M>();
            foreach (E dbEntity in dbEntities)
            {
                M model = new M();
                model.Instantiate(dbEntity);
                models.Add(model);
            }

            return models;
        }

        // TODO: Comment.
        public Model()
        {
            Id = ID_WHEN_NOT_CREATED_IN_DB;
        }

        public abstract void Instantiate(E dbEntity);

    }
}