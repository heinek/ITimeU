using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class RaceAthleteViewModel
    {
        public List<AthleteModel> AthletesAvailable { get; set; }
        public List<AthleteModel> AthletesConnected { get; set; }
        public string SavedConnected { get; set; }
        public int RaceId { get; set; }

        public int[] AvailableSelected { get; set; }
        public int[] ConnectedSelected { get; set; }

    }
}