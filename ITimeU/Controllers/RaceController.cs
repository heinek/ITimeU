using System;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class RaceController : Controller
    {
        //
        // GET: /Race/
        [HttpGet]
        public ActionResult Index(int? eventId)
        {
            if (eventId.HasValue)
                ViewBag.EventId = eventId.Value;
            ViewBag.Events = EventModel.GetEvents();
            return View();
        }

        [HttpPost]
        public ActionResult Index(RaceModel model)
        {
            ViewBag.Events = EventModel.GetEvents();
            if (model.StartDate < DateTime.Today)
            {
                ViewBag.Error = "Dato kan ikke være mindre enn dagens dato";
                return View();
            }
            var race = new RaceModel();
            race.Name = model.Name;
            race.Distance = model.Distance;
            race.StartDate = model.StartDate;
            race.EventId = model.EventId;
            try
            {
                if (race.Save())
                {
                    var checkpoint = new CheckpointModel("Mål", race.RaceId);
                    checkpoint.Sortorder = 99;
                    checkpoint.SaveToDb();
                    ViewData.ModelState.Clear();
                    ViewBag.Success = "Løp ble opprettet";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Det skjedde en feil under lagring av løp";
                    return View();
                }
            }
            catch (ArgumentException ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
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

        public ActionResult Delete(int raceid)
        {
            var race = RaceModel.GetById(raceid);
            race.Delete();
            return View("List", RaceModel.GetRaces());
        }

        public ActionResult List(int? eventId)
        {
            if (eventId.HasValue)
                return View("List", RaceModel.GetRaces(eventId.Value));
            return View("List", RaceModel.GetRaces());
        }
        public ActionResult Setup()
        {
            return View("Setup");
        }
    }
}
