using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace ITimeU.Models
{
    public class ClubModel : Model<Club>
    {
        private static Entities entitiesStatic = new Entities();
        private static ModelFactory<Club, ClubModel> modelFactory = new ModelFactory<Club, ClubModel>(entitiesStatic, entitiesStatic.Clubs);
        
        public string Name { get; private set; }

        public ClubModel() {
            this.Name = null;
        }

        public ClubModel(string name)
        {
            this.Name = name;
        }

        public ClubModel(Club club)
        {
            InstantiateFrom(club);
        }

        public override void InstantiateFrom(Club club)
        {
            Id = club.ClubID;
            Name = club.Name;
        }

        protected override ObjectSet<Club> GetModelObjectSetFrom(Entities entities)
        {
            return entities.Clubs;
        }

        protected override Club GetMeIn(ObjectSet<Club> dbObjectSet)
        {
            return dbObjectSet.Where(club => club.ClubID == Id).Single();
        }

        // TODO: Move to NamedModel
        protected override int GetOrCreateDbEntity()
        {
            Club clubDb;
            try
            {
                clubDb = ClubModel.GetDbEntityBy(Name);
            }
            catch (InvalidOperationException)
            {
                clubDb = ClubModel.CreateDbEntity(Name);
            }

            return ClubModel.GetIdFrom(clubDb);
        }

        protected override void updateDbEntity()
        {
            Club club = entitiesStatic.Clubs.Single(enitity => enitity.ClubID == Id);
            updateProperties(club);
            entitiesStatic.SaveChanges();
        }

        private void updateProperties(Club club)
        {
            club.Name = Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ClubModel other = (ClubModel)obj;

            return Id == other.Id && Name.Equals(other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static int GetIdFrom(Club club) {
            return club.ClubID;
        }

        public static List<ClubModel> GetAll()
        {
            return modelFactory.GetAll();
        }

        /// <summary>
        /// Returns the club with the given name, or creates a new one
        /// if none is found in the database.
        /// </summary>
        /// <param name="name">The name of the club to get/create.</param>
        /// <returns>The existing or newly created club.</returns>
        // TODO: Move to NamedModel
        public static ClubModel GetOrCreate(string name)
        {
            Club clubDb = null;

            try
            {
                clubDb = GetDbEntityBy(name);
            }
            catch (InvalidOperationException)
            {
                clubDb = CreateDbEntity(name);
            }
          
            return new ClubModel(clubDb);
        }

        private static Club GetDbEntityBy(string name)
        {
            return entitiesStatic.Clubs.Single(club => club.Name == name);
        }

        private static Club CreateDbEntity(string name)
        {
            Club clubDb = new Club();
            clubDb.Name = name;
            modelFactory.SaveEntityToDb(clubDb);
            
            return clubDb;
        }
        
        public static ClubModel GetByName(string name)
        {
            try
            {
                return TryToGetByName(name);
            }
            catch (InvalidOperationException)
            {
                throw new ModelNotFoundException(typeof(ClubModel).Name + " with name " + name + " not found in database.");
            }
        }

        private static ClubModel TryToGetByName(string name)
        {
            Club dbEntity = entitiesStatic.Clubs.Single(temp => temp.Name == name);
            return new ClubModel(dbEntity);
        }

    }
}