
namespace ITimeU.Models
{
    public class StartRaceViewModel
    {
        public int RaceId { get; set; }
        public int EvetntId { get; set; }

        public StartRaceViewModel()
        {

        }
        public StartRaceViewModel(int raceid)
        {
            RaceId = raceid;
        }
    }
}