using System;
using System.Collections.Generic;
using System.Linq;


namespace ITimeU.Models
{
    [Serializable]
    public class AthleteModel
    {
        private const int EMPTY_ID = -1;
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int? Birthday { get; private set; }
        public ClubModel Club { get; private set; }
        public AthleteClassModel AthleteClass { get; private set; }
        public int? StartNumber { get; private set; }

        private bool dbEntryCreated
        {
            get
            {
                if (Id == EMPTY_ID)
                    return false;
                return true;
            }
        }

        public static AthleteModel GetById(int idToGet)
        {
            var entities = new Entities();
            try
            {
                return TryToGetById(idToGet, entities);
            }
            catch (InvalidOperationException e)
            {
                throw new ModelNotFoundException("Athlete with ID " + idToGet + " not found in database.");
            }
        }

        private static AthleteModel TryToGetById(int idToGet, Entities entities)
        {
            Athlete athleteDb = entities.Athletes.Single(temp => temp.ID == idToGet);
            return new AthleteModel(athleteDb);
        }

        /// <summary>
        /// Retrieves all athletes in the database.
        /// </summary>
        /// <returns></returns>
        public static List<AthleteModel> GetAll()
        {
            var entities = new Entities();
            IEnumerable<Athlete> athletes = entities.Athletes.AsEnumerable<Athlete>();

            List<AthleteModel> athleteModels = new List<AthleteModel>();
            foreach (Athlete athleteDb in athletes)
            {
                AthleteModel athleteModel = new AthleteModel(athleteDb);
                athleteModels.Add(athleteModel);
            }

            return athleteModels;
        }
        
        public AthleteModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            SetDefaultId();
        }

        private void SetDefaultId()
        {
            Id = EMPTY_ID;
        }

        public AthleteModel(Athlete athleteDb)
        {
            Id = athleteDb.ID;
            FirstName = athleteDb.FirstName;
            LastName = athleteDb.LastName;
            Birthday = athleteDb.Birthday;
            if (athleteDb.Club != null)
                Club = ClubModel.GetOrCreate(athleteDb.Club.Name);
            if (athleteDb.AthleteClass != null)
                AthleteClass = AthleteClassModel.GetOrCreate(athleteDb.AthleteClass.Name);
            if (athleteDb.Startnumber != null)
                StartNumber = athleteDb.Startnumber;
        }

        public AthleteModel(string firstName, string lastName, int? birthday, ClubModel club, AthleteClassModel athleteClass, int? startNumber)
        {
            SetDefaultId();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Club = club;
            AthleteClass = athleteClass;
            StartNumber = startNumber;
        }

        /// <summary>
        /// Saves a list of athletes to the database.
        /// </summary>
        /// <param name="athletes">The list of athletes to save.</param>
        public static void SaveToDb(List<AthleteModel> athletes)
        {
            Entities context = new Entities();
            foreach (AthleteModel athlete in athletes)
            {
                Athlete athleteDb = createAthleteDbFrom(athlete);
                context.Athletes.AddObject(athleteDb);
            }
            context.SaveChanges();
        }

        private static Athlete createAthleteDbFrom(AthleteModel athlete)
        {
            Athlete athleteDb = new Athlete();
            athleteDb.FirstName = athlete.FirstName;
            athleteDb.LastName = athleteDb.LastName;
            return athleteDb;
        }

        public void SaveToDb()
        {
            var context = new Entities();

            if (!dbEntryCreated)
                Id = CreateDbEntity(context);
            else
                updateDbEntity(context);
        }


        private int CreateDbEntity(Entities context)
        {
            Athlete athlete = new Athlete();
            updateProperties(athlete);
            context.Athletes.AddObject(athlete);
            context.SaveChanges();

            return athlete.ID;
        }

        private void updateProperties(Athlete athlete)
        {
            athlete.FirstName = FirstName;
            athlete.LastName = LastName;
            athlete.Birthday = Birthday;
            if (Club != null)
                athlete.ClubID = Club.SaveToDb();
            if (AthleteClass != null)
                athlete.ClassID = AthleteClass.SaveToDb();

           // athlete.AthleteClass = AthleteClass;
            athlete.Startnumber = StartNumber;

        }

        private void updateDbEntity(Entities context)
        {
            Athlete athlete = context.Athletes.Single(enitity => enitity.ID == Id);
            updateProperties(athlete);
            context.SaveChanges();
        }

        public override string ToString()
        {
            return FirstName + " " + LastName ;
        }


        /// <summary>
        /// Deletes the given athlete.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public void DeleteFromDb()
        {
            using (var ctx = new Entities())
            {
                var rowToDelete = ctx.Athletes.Where(runt => runt.ID == Id).Single();
                ctx.Athletes.DeleteObject(rowToDelete);
                ctx.SaveChanges();
            }

            SetDefaultId();
        }

    }
}
