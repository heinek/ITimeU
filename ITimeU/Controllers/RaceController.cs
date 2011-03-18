using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            RaceModel race = new RaceModel();
            int intDistance;            
            int.TryParse(distance, out intDistance);
            Race newRace = new Race();
            newRace.Name = name;
            newRace.Distance = intDistance;
            newRace.StartDate = Convert.ToDateTime(startDate);
            race.InsertRace(newRace);
            return View("Index");           
        }
    }
}
