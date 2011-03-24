using System;
using System.Collections.Generic;
using System.Linq;

namespace ITimeU.Models
{
    public class RaceModel
    {
        public int RaceId { get; private set; }
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public int? Distance { get; set; }

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
            Name = raceDb.Name;
            StartDate = raceDb.StartDate;
            Distance = raceDb.Distance;
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
    }
}
