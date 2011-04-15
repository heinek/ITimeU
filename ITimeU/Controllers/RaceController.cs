using System;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class RaceController : Controller
    {
        //
        // GET: /Race/

        public ActionResult Index()
        {
            ViewBag.Error = false;
            ViewBag.Events = EventModel.GetEvents();
            return View();
        }

        public ActionResult Create(string name, string distance, string startDate, int eventId)
        {
            int intDistance;
            int.TryParse(distance, out intDistance);
            var race = new RaceModel();
            race.Name = name;
            race.Distance = intDistance;
            race.StartDate = Convert.ToDateTime(startDate);
            race.EventId = eventId;
            race.Save();
            var checkpoint = new CheckpointModel("Mål", race.RaceId);
            checkpoint.Sortorder = 99;
            checkpoint.SaveToDb();
            return View("Index");
        }

        private bool IsValidInput(Race checkRace)
        {
            var check = true;

            if (String.IsNullOrEmpty(checkRace.Name))
                check = false;
            return check;
        }

        public ActionResult List(int eventId)
        {
            return View("List", RaceModel.GetRaces(eventId));
        }
        public ActionResult Setup()
        {
            return View("Setup");
        }
    }
}
