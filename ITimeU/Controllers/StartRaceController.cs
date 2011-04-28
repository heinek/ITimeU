using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class StartRaceController : Controller
    {
        //
        // GET: /StartRace/

        public ActionResult Index()
        {
            ViewBag.Events = EventModel.GetEvents();
            return View("Index");
        }

        public ActionResult SelectRace(int eventId)
        {
            ViewBag.Races = RaceModel.GetRaces(eventId);
            ViewBag.EventId = eventId;
            return View("SelectRace");
        }
        public ActionResult ComputerSetup(int raceid)
        {
            var race = RaceModel.GetById(raceid);
            var startRace = new StartRaceViewModel(raceid);
            startRace.EvetntId = race.EventId;
            return View("ComputerSetup", startRace);
        }

        public ActionResult DualPcSetup(int raceid)
        {
            var startRace = new StartRaceViewModel(raceid);
            ViewBag.RaceId = raceid;
            return View("DualPcSetup", startRace);
        }
    }
}
