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

    }
}
