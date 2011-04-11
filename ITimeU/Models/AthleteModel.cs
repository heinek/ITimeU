using System;
using System.Collections.Generic;
using System.Linq;


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
            Athlete athleteDb = entities.Athletes.Single(temp => temp.ID == idToGet && temp.IsDeleted.Value != true);
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

        public AthleteModel()
        {
            // TODO: Complete member initialization
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
            RaceAthlete raceAthlete = new RaceAthlete();
            raceAthlete.AthleteId = Id;
            raceAthlete.RaceId = raceId;

            if (StartNumber.HasValue) raceAthlete.Startnumber = StartNumber.Value;
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

        /// <summary>
        /// Startnumbers the exists in db.
        /// </summary>
        /// <param name="startnumber">The startnumber.</param>
        /// <returns></returns>
        public static bool StartnumberExistsInDb(int startnumber)
        {
            using (var context = new Entities())
            {
                return context.Athletes.Where(athlete => athlete.IsDeleted == false).Any(athlete => athlete.Startnumber.Value == startnumber);
            }
        }

        public void Delete()
        {
            using (var context = new Entities())
            {
                var athleteToDelete = context.Athletes.Where(athlete => athlete.ID == Id).Single();
                athleteToDelete.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public static List<AthleteModel> GetAthletes(int clubId)
        {
            using (var context = new Entities())
            {
                return context.Athletes.
                    Where(athlete => athlete.ClubID.HasValue && athlete.ClubID.Value == clubId).
                    Select(athlete => new AthleteModel()
                {
                    Id = athlete.ID,
                    FirstName = athlete.FirstName,
                    LastName = athlete.LastName,
                    Email = athlete.Email,
                    Birthday = athlete.Birthday.HasValue ? athlete.Birthday.Value : 0,
                    //Club = athlete.ClubID.HasValue ?
                    //new ClubModel()
                    //{
                    //    Id = athlete.ClubID.Value,
                    //    Name = athlete.Club.Name
                    //}
                    //: null,
                    Gender = athlete.Gender,
                    PhoneNumber = athlete.Phone,
                    PostalAddress = athlete.PostalAddress,
                    PostalCode = athlete.PostalCode,
                    StartNumber = athlete.Startnumber.HasValue ? athlete.Startnumber.Value : 0
                    //AthleteClass = athlete.ClassID.HasValue
                    //? new AthleteClassModel()
                    //{
                    //    Id = athlete.ClassID.Value,
                    //    Name = athlete.AthleteClass.Name
                    //}
                    //: null
                }).ToList();
            }
        }

        public static List<AthleteModel> GetAthletesForRace(int raceId)
        {
            var athletes = new List<AthleteModel>();
            using (var context = new Entities())
            {
                var raceathletes = context.RaceAthletes.Where(ra => ra.RaceId == raceId).ToList();
                foreach (var raceathlete in raceathletes)
                {
                    var athlete = new AthleteModel()
                    {
                        Id = raceathlete.Athlete.ID,
                        FirstName = raceathlete.Athlete.FirstName,
                        LastName = raceathlete.Athlete.LastName,
                        Email = raceathlete.Athlete.Email,
                        Birthday = raceathlete.Athlete.Birthday.HasValue ? raceathlete.Athlete.Birthday.Value : 0,
                        Gender = raceathlete.Athlete.Gender,
                        PhoneNumber = raceathlete.Athlete.Phone,
                        PostalAddress = raceathlete.Athlete.PostalAddress,
                        PostalCode = raceathlete.Athlete.PostalCode,
                        StartNumber = raceathlete.Athlete.Startnumber.HasValue ? raceathlete.Athlete.Startnumber.Value : 0
                    };
                    athletes.Add(athlete);
                }
            }
            return athletes;
        }
    }
}
