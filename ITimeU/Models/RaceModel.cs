using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITimeU.Models
{
    public class RaceModel
    {
        public int RaceId { get; set; }
        [Required(ErrorMessage = "Løpsnavn er obligatorisk")]
        [DisplayName("Navn")]
        [StringLength(150, ErrorMessage = "Løpsnavn kan ikke være lengre enn 150 bokstaver")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Dato er obligatorisk")]
        [DataType(DataType.Date)]
        [DisplayName("Dato")]
        public DateTime StartDate { get; set; }
        [DisplayName("Distanse")]
        public int? Distance { get; set; }
        [DisplayName("Stevne")]
        public int EventId { get; set; }
        public EventModel Event { get; set; }

        public RaceModel()
        {

        }

        public RaceModel(string name, DateTime startDate)
        {
            this.Name = name;
            this.StartDate = startDate;
        }

        public RaceModel(Race raceDb)
        {
            RaceId = raceDb.RaceID;
            if (raceDb.EventId.HasValue)
            {
                EventId = raceDb.EventId.Value;
                Event = EventModel.GetById(raceDb.EventId.Value);
            }
            Name = raceDb.Name;
            StartDate = raceDb.StartDate;
            Distance = raceDb.Distance;
        }

        public bool Save()
        {
            using (var context = new Entities())
            {
                if (context.Races.Where(r => r.EventId == EventId).Any(r => r.Name == Name)) throw new ArgumentException("Det eksisterer allerede et løp med samme navn for dette stevnet");
                var race = new Race();
                race.Name = Name;
                race.StartDate = StartDate;
                race.Distance = Distance;
                race.EventId = EventId;
                context.Races.AddObject(race);
                try
                {
                    context.SaveChanges();
                    this.RaceId = race.RaceID;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static List<RaceModel> GetRaces()
        {
            using (var ctx = new Entities())
            {
                return ctx.Races.Where(race => !race.IsDeleted).Select(race =>
                    new RaceModel()
                    {
                        RaceId = race.RaceID,
                        Name = race.Name,
                        Distance = race.Distance.HasValue ? race.Distance.Value : 0,
                        StartDate = race.StartDate,
                        EventId = race.EventId.Value,
                        Event = new EventModel()
                        {
                            EventId = race.EventId.Value,
                            Name = race.Event.Name,
                            EventDate = race.Event.EventDate
                        }
                    }).ToList();
            }
        }

        public static List<RaceModel> GetRaces(int eventId)
        {
            using (var ctx = new Entities())
            {
                return ctx.Races.Where(race => !race.IsDeleted && race.EventId == eventId).Select(race =>
                    new RaceModel()
                    {
                        RaceId = race.RaceID,
                        Name = race.Name,
                        StartDate = race.StartDate,
                        Distance = race.Distance.HasValue ? race.Distance.Value : 0,
                        EventId = race.EventId.Value,
                        Event = new EventModel()
                        {
                            EventId = race.EventId.Value,
                            Name = race.Event.Name,
                            EventDate = race.Event.EventDate
                        }
                    }).ToList();
            }
        }
        //public void InsertRace(Race races)
        //{

        //    using (var ctxDB = new Entities())
        //    {                                
        //        races.IsDeleted = false;
        //        ctxDB.Races.AddObject(races);
        //        ctxDB.SaveChanges();
        //    }

        //}

        public void UpdateRaceName(string name)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == RaceId).Single();
                select.Name = name;
                ctxDB.SaveChanges();
            }
        }


        public void UpdateRaceDistance(int distance)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == RaceId).Single();
                select.Distance = distance;
                ctxDB.SaveChanges();
            }
        }

        public void UpdateRaceDate(DateTime date)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == RaceId).Single();
                select.StartDate = date;
                ctxDB.SaveChanges();
            }
        }


        public static Race GetRace(int id)
        {
            Race select;
            using (var ctxDB = new Entities())
            {
                select = ctxDB.Races.Where(race => race.RaceID == id).Single();
            }
            return select;
        }

        public static RaceModel GetById(int idToGet)
        {
            var entities = new Entities();
            try
            {
                return TryToGetById(idToGet, entities);
            }
            catch (InvalidOperationException)
            {
                throw new ModelNotFoundException(typeof(RaceModel).Name + " with ID " + idToGet + " not found in database.");
            }
        }

        private static RaceModel TryToGetById(int idToGet, Entities entities)
        {
            Race raceDb = entities.Races.Single(temp => temp.RaceID == idToGet);
            return new RaceModel(raceDb);
        }

        public List<AthleteModel> GetAthletes()
        {
            var athletes = new List<AthleteModel>();
            using (var ctx = new Entities())
            {
                foreach (var athlete in ctx.RaceAthletes.Where(raceathlete => raceathlete.RaceId == this.RaceId))
                {
                    athletes.Add(new AthleteModel()
                    {
                        Id = athlete.Athlete.ID,
                        FirstName = athlete.Athlete.FirstName,
                        LastName = athlete.Athlete.LastName,
                        StartNumber = athlete.Athlete.Startnumber.HasValue ? athlete.Athlete.Startnumber.Value : 0,
                        AthleteClass = athlete.Athlete.ClassID.HasValue ? new AthleteClassModel()
                        {
                            Id = athlete.Athlete.AthleteClass.ID,
                            Name = athlete.Athlete.AthleteClass.Name
                        } : null,
                        Birthday = athlete.Athlete.Birthday.HasValue ? athlete.Athlete.Birthday.Value : 0,
                        Club = athlete.Athlete.ClubID.HasValue ? new ClubModel()
                        {
                            Id = athlete.Athlete.Club.ClubID,
                            Name = athlete.Athlete.Club.Name
                        } : null
                    });
                }
            }
            return athletes;
        }

        public List<AthleteModel> GetAthletesNotConnected()
        {
            var athletes = new List<AthleteModel>();
            using (var ctx = new Entities())
            {
                var athletesQuery = from a in ctx.Athletes
                                    where !(from ra in ctx.RaceAthletes select ra.AthleteId).Contains(a.ID)
                                    select a;

                foreach (var athlete in athletesQuery.Where(a => !a.IsDeleted.HasValue || a.IsDeleted.Value == false))
                {
                    athletes.Add(new AthleteModel()
                    {
                        Id = athlete.ID,
                        FirstName = athlete.FirstName,
                        LastName = athlete.LastName,
                        StartNumber = athlete.Startnumber.HasValue ? athlete.Startnumber.Value : 0,
                        AthleteClass = athlete.ClassID.HasValue ? new AthleteClassModel()
                        {
                            Id = athlete.AthleteClass.ID,
                            Name = athlete.AthleteClass.Name
                        } : null,
                        Birthday = athlete.Birthday.HasValue ? athlete.Birthday.Value : 0,
                        Club = athlete.ClubID.HasValue ? new ClubModel()
                        {
                            Id = athlete.Club.ClubID,
                            Name = athlete.Club.Name
                        } : null
                    });
                }
            }
            return athletes;
        }

        public bool HasTimer()
        {
            using (var context = new Entities())
            {
                if (context.Races.Where(race => race.RaceID == RaceId).Single().Timers.Count > 0)
                    return true;
                return false;
            }
        }

        public int? GetTimerId()
        {
            using (var context = new Entities())
            {
                var timer = context.Races.Where(race => race.RaceID == RaceId).Single().Timers.SingleOrDefault();
                if (timer != null)
                    return timer.TimerID;
                return null;
            }
        }

        public void Delete()
        {
            using (var context = new Entities())
            {
                context.Races.Where(race => race.RaceID == RaceId).Single().IsDeleted = true;
                context.SaveChanges();
            }
        }

    }
}
