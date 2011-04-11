using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ITimeU.Models
{
    [Serializable]
    public class AthleteModel
    {
        private const int EMPTY_ID = -1;
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string PostalAddress { get; private set; }
        public string PostalCode { get; private set; }
        public string City { get; private set; }
        public string Email { get; private set; }
        public string Gender { get; private set; }
        public string PhoneNumber { get; private set; }

        public int? Birthday { get; set; }
        public ClubModel Club { get; set; }
        public AthleteClassModel AthleteClass { get; set; }
        public int? StartNumber { get; set; }

        public Dictionary<int, string> AthleteDic { get; set; }

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
            catch (InvalidOperationException)
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

            return athleteModels.OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
        }

        public List<AthleteModel> GetAllByClubId(int id)
        {
            var entities = new Entities();
            IEnumerable<Athlete> athletes = entities.Athletes.AsEnumerable<Athlete>();

            List<AthleteModel> athleteModels = new List<AthleteModel>();
            AthleteDic.Clear();
            foreach (Athlete athleteDb in athletes)
            {
                if (athleteDb.ClubID == id)
                {
                    AthleteModel athleteModel = new AthleteModel(athleteDb);
                    athleteModels.Add(athleteModel);
                    AthleteDic.Add(athleteModel.Id, athleteModel.FullName);
                    
                }
            }                       

            return athleteModels.OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
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
            Email = athleteDb.Email;
            PostalAddress = athleteDb.PostalAddress;
            PostalCode = athleteDb.PostalCode;
            City = athleteDb.PostalPlace;
            PhoneNumber = athleteDb.Phone;
            Gender = athleteDb.Gender;
            Birthday = athleteDb.Birthday;


            if (athleteDb.Club != null)
                Club = ClubModel.GetOrCreate(athleteDb.Club.Name);
            if (athleteDb.AthleteClass != null)
                AthleteClass = AthleteClassModel.GetOrCreate(athleteDb.AthleteClass.Name);
            if (athleteDb.Startnumber != null)
                StartNumber = athleteDb.Startnumber;
        }

        public AthleteModel(string firstName, string lastName, int? birthday, ClubModel club, AthleteClassModel athleteClass, int? startNumber, int id = EMPTY_ID)
        {
            SetDefaultId();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Club = club;
            AthleteClass = athleteClass;
            StartNumber = startNumber;
        }

        public AthleteModel(string firstName, string lastName, string email, string address, string postalcode, string city, string gender, int? birthday, string phonenumber, int? startNumber, ClubModel club, AthleteClassModel athleteClass)
        {
            SetDefaultId();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            Club = club;
            AthleteClass = athleteClass;
            StartNumber = startNumber;
            PostalAddress = address;
            PostalCode = postalcode;
            City = city;
            Gender = gender;
            PhoneNumber = phonenumber;
            Email = email;
            
        }

        public AthleteModel()
        {
            // TODO: Complete member initialization
            AthleteDic = new Dictionary<int, string>();
        }

        public AthleteModel(int id)
        {
            Id = id;
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
            athleteDb.LastName = athlete.LastName;
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
            athlete.PostalAddress = PostalAddress;
            athlete.PostalCode = PostalCode;
            athlete.PostalPlace = City;
            athlete.Email = Email;
            athlete.Gender = Gender;
            athlete.Phone = PhoneNumber;
            athlete.IsDeleted = false;

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
            return FirstName + " " + LastName;
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


        public void ConnectToRace(int raceId)
        {
            Entities context = new Entities();
            Athlete athleteDb = context.Athletes.Single(temp => temp.ID == Id);
            RaceAthlete raceAthlete = new RaceAthlete();
            raceAthlete.AthleteId = Id;
            raceAthlete.RaceId = raceId;

            if (athleteDb.Startnumber.HasValue) raceAthlete.Startnumber = athleteDb.Startnumber.Value;
            context.RaceAthletes.AddObject(raceAthlete);
            context.SaveChanges();
        }

        public void RemoveFromRace(int raceid)
        {
            using (var ctx = new Entities())
            {
                var athleteToRemove = ctx.RaceAthletes.Where(ra => ra.AthleteId == Id && ra.RaceId == raceid).SingleOrDefault();
                ctx.RaceAthletes.DeleteObject(athleteToRemove);
                ctx.SaveChanges();
            }
        }
    }
}
