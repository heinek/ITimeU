using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ViewBag.Races = RaceModel.GetRaces();
            return View("Index");
        }

        public ActionResult ComputerSetup(int raceid)
        {
            var startRace = new StartRaceViewModel(raceid);
            return View("ComputerSetup", startRace);
        }

        public ActionResult DualPcSetup(int raceid)
        {
            var startRace = new StartRaceViewModel(raceid);
            return View("DualPcSetup", startRace);
        }
    }
}
