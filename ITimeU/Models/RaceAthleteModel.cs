using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class RaceAthleteModel
    {
        public int RaceId { get; set; }
        public RaceModel RaceModel { get; set; }
        public int AthleteId { get; set; }
        public AthleteModel AthleteModel { get; set; }
        public int? Startnumber { get; set; }

        public RaceAthleteModel()
        {

        }
        public static List<RaceAthleteModel> GetRaceAthletesForRace(int raceid)
        {
            using (var context = new Entities())
            {
                return context.RaceAthletes.
                    Where(raceAthlete => raceAthlete.RaceId == raceid).
                    Select(raceAthlete => new RaceAthleteModel()
                    {
                        RaceId = raceAthlete.RaceId,
                        RaceModel = new RaceModel()
                        {
                            RaceId = raceAthlete.RaceId,
                            Name = raceAthlete.Race.Name,
                            Distance = raceAthlete.Race.Distance.HasValue ? raceAthlete.Race.Distance.Value : 0,
                            StartDate = raceAthlete.Race.StartDate
                        },
                        AthleteId = raceAthlete.AthleteId,
                        AthleteModel = new AthleteModel()
                        {
                            Id = raceAthlete.AthleteId,
                            FirstName = raceAthlete.Athlete.FirstName,
                            LastName = raceAthlete.Athlete.LastName,
                            Birthday = raceAthlete.Athlete.Birthday,
                            StartNumber = raceAthlete.Athlete.Startnumber
                        },
                        Startnumber = raceAthlete.Startnumber
                    }
                    ).ToList();
            }
        }
    }
}