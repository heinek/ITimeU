using System;
using System.Collections.Generic;
using System.Linq;

namespace ITimeU.Models
{
    public class RaceModel
    {
        public int RaceId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int Distance { get; set; }

        public RaceModel()
        {

        }

        public RaceModel(string name, DateTime startDate)
        {
            this.Name = name;
            this.StartDate = startDate;
        }

        public bool Save()
        {
            using (var context = new Entities())
            {
                var race = new Race();
                race.Name = Name;
                race.StartDate = StartDate;
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
                        StartDate = race.StartDate
                    }).ToList();
            }
        }

        public void InsertRace(Race races)
        {

            using (var ctxDB = new Entities())
            {
                races.IsDeleted = false;
                ctxDB.Races.AddObject(races);
                ctxDB.SaveChanges();
            }

        }

        public void UpdateRaceName(int id, string name)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == id).Single();
                select.Name = name;
                ctxDB.SaveChanges();
            }
        }


        public void UpdateRaceDistance(int id, int distance)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == id).Single();
                select.Distance = distance;
                ctxDB.SaveChanges();
            }
        }

        public void UpdateRaceDate(int id, DateTime date)
        {
            using (var ctxDB = new Entities())
            {
                var select = ctxDB.Races.Where(race => race.RaceID == id).Single();
                select.StartDate = date;
                ctxDB.SaveChanges();
            }
        }


        public Race GetRace(int id)
        {
            Race select;
            using (var ctxDB = new Entities())
            {
                select = ctxDB.Races.Where(race => race.RaceID == id).Single();
            }
            return select;
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

        internal static RaceModel GetById(int raceid)
        {
            using (var entities = new Entities())
            {
                var race = entities.Races.Where(r => r.RaceID == raceid).Single();
                return new RaceModel()
                {
                    RaceId = race.RaceID,
                    Name = race.Name,
                    Distance = race.Distance.HasValue ? race.Distance.Value : 0,
                    StartDate = race.StartDate
                };
            }
        }

    }
}
