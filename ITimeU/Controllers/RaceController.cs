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
            return View();
        }

        public ActionResult Create(string name, string distance, string startDate)
        {
            int intDistance;
            int.TryParse(distance, out intDistance);
            var race = new RaceModel();
            race.Name = name;
            race.Distance = intDistance;
            race.StartDate = Convert.ToDateTime(startDate);
            race.Save();
            return View("Index");
        }

        private bool IsValidInput(Race checkRace)
        {
            var check = true;

            if (String.IsNullOrEmpty(checkRace.Name))
                check = false;
            return check;
        }
    }
}
