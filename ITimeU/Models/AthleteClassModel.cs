using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class AthleteClassModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private static Entities entitiesStatic = new Entities();
        private AthleteClass athleteClass;

        private bool dbEntryCreated
        {
            get
            {
                if (Id == 0)
                    return false;
                return true;
            }
        }

        public AthleteClassModel(string name)
        {
            Name = name;
            Id = 0;
        }

        public AthleteClassModel(AthleteClass athleteClass)
        {
            Id = athleteClass.ID;
            Name = athleteClass.Name;
        }

        internal int SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntity(context);

            return Id;
        }

        private int CreateDbEntity(Entities context)
        {
            AthleteClass athleteClass = new AthleteClass();
            updateProperties(athleteClass);
            context.AthleteClasses.AddObject(athleteClass);
            context.SaveChanges();

            return athleteClass.ID;
        }

        private void updateProperties(AthleteClass athleteClass)
        {
            athleteClass.Name = Name;
        }

        private void updateDbEntity(Entities context)
        {
            AthleteClass athleteClass = context.AthleteClasses.Single(enitity => enitity.ID == Id);
            updateProperties(athleteClass);
            context.SaveChanges();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            AthleteClassModel other = (AthleteClassModel)obj;

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
        /// Retrieves all athlete classes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<AthleteClassModel> GetAll()
        {
            IEnumerable<AthleteClass> athleteClasses = entitiesStatic.AthleteClasses.AsEnumerable<AthleteClass>();

            List<AthleteClassModel> athleteClassModels = new List<AthleteClassModel>();
            foreach (AthleteClass athleteClassDb in athleteClasses)
            {
                AthleteClassModel athleteClass = new AthleteClassModel(athleteClassDb);
                athleteClassModels.Add(athleteClass);
            }

            return athleteClassModels;
        }


        public static AthleteClassModel GetOrCreate(string name)
        {
            AthleteClass athleteClassDb = null;

            try
            {
                athleteClassDb = getDbEntry(name);
            }
            catch (InvalidOperationException)
            {
                athleteClassDb = createDbEntry(name);
            }

            return new AthleteClassModel(athleteClassDb);
        }

        private static AthleteClass getDbEntry(string name)
        {
            return entitiesStatic.AthleteClasses.Single(temp => temp.Name == name);
        }

        private static AthleteClass createDbEntry(string name)
        {
            AthleteClass athleteClassDb = new AthleteClass();
            athleteClassDb.Name = name;
            saveDbEntry(athleteClassDb);

            return athleteClassDb;
        }

        private static void saveDbEntry(AthleteClass athleteClassDb)
        {
            entitiesStatic.AthleteClasses.AddObject(athleteClassDb);
            entitiesStatic.SaveChanges();
        }

        public static void DeleteIfExists(string name)
        {
            try
            {
                AthleteClass athleteClassDb = entitiesStatic.AthleteClasses.Single(temp => temp.Name == name);
                entitiesStatic.AthleteClasses.DeleteObject(athleteClassDb);
                entitiesStatic.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                // No DB entry found, do noting
            }
        }
    }
}