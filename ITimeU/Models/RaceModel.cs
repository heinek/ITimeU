using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITimeU.Models
{
    public class RaceModel
    {
        public int RaceId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public RaceModel()
        {

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
