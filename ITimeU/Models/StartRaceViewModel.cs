using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class StartRaceViewModel
    {
        public int RaceId { get; set; }

        public StartRaceViewModel()
        {

        }
        public StartRaceViewModel(int raceid)
        {
            RaceId = raceid;
        }
    }
}