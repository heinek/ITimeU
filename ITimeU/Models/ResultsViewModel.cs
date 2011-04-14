using System.Collections.Generic;

namespace ITimeU.Models
{
    public class ResultsViewModel
    {
        public string Checkpointname { get; set; }
        //public int Rank { get; set; }
        public int Startnumber { get; set; }
        public string Fullname { get; set; }
        public string Clubname { get; set; }
        public string Time { get; set; }

        private Dictionary<string, int> rankIterators = new Dictionary<string, int>();

        public ResultsViewModel()
        {

        }
        public ResultsViewModel(string checkpointname, int startnumber, string fullname, string clubname, string time)
        {
            Checkpointname = checkpointname;
            if (rankIterators.ContainsKey(checkpointname))
                rankIterators[checkpointname] = rankIterators[checkpointname] + 1;
            else
                rankIterators.Add(checkpointname, 1);
            //Rank = rankIterators[checkpointname];
            Startnumber = startnumber;
            Fullname = fullname;
            Clubname = clubname;
            Time = time;
        }
    }
}