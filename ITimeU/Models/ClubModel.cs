using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class ClubModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private static Entities entitiesStatic = new Entities();

        private bool dbEntryCreated
        {
            get
            {
                if (Id == 0)
                    return false;
                return true;
            }
        }

        public ClubModel(string name)
        {
            this.Name = name;
            Id = 0;
        }

        private ClubModel(Club club)
        {
            Id = club.ClubID;
            Name = club.Name;
        }

        internal int SaveToDb()
        {
            var entities = new Entities();
            if (!dbEntryCreated)
                Id = CreateDbEntity(entities);
            else
                updateDbEntity(entities);

            return Id;
        }

        private int CreateDbEntity(Entities entities)
        {
            Club club = new Club();
            updateProperties(club);
            entities.Clubs.AddObject(club);
            entities.SaveChanges();

            return club.ClubID;
        }

        private void updateProperties(Club club)
        {
            club.Name = Name;
        }

        private void updateDbEntity(Entities entities)
        {
            Club club = entities.Clubs.Single(enitity => enitity.ClubID == Id);
            updateProperties(club);
            entities.SaveChanges();
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

        /// <summary>
        /// Retrieves all clubs in the database.
        /// </summary>
        /// <returns></returns>
        public static List<ClubModel> GetAll()
        {
            IEnumerable<Club> clubs = entitiesStatic.Clubs.AsEnumerable<Club>();

            List<ClubModel> clubModels = new List<ClubModel>();
            foreach (Club clubDb in clubs)
            {
                ClubModel clubModel = new ClubModel(clubDb);
                clubModels.Add(clubModel);
            }

            return clubModels;
        }

        public static ClubModel GetOrCreate(string name)
        {
            Club clubDb = null;

            try
            {
                clubDb = getDbEntry(name);
            }
            catch (InvalidOperationException e)
            {
                clubDb = createDbEntry(name);
            }
          
            return new ClubModel(clubDb);
        }

        private static Club getDbEntry(string name)
        {
            return entitiesStatic.Clubs.Single(temp => temp.Name == name);
        }

        private static Club createDbEntry(string name)
        {
            Club clubDb = new Club();
            clubDb.Name = name;
            saveDbEntry(clubDb);

            return clubDb;
        }

        private static void saveDbEntry(Club clubDb)
        {
            entitiesStatic.Clubs.AddObject(clubDb);
            entitiesStatic.SaveChanges();
        }

        public static void DeleteIfExists(string name)
        {
            try
            {
                Club clubDb = entitiesStatic.Clubs.Single(temp => temp.Name == name);
                entitiesStatic.Clubs.DeleteObject(clubDb);
                entitiesStatic.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                // No DB entry found, do noting
            }
        }
    }
}