using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class RaceAthleteController : Controller
    {
        //
        // GET: /RaceAthlete/

        [HttpGet]
        public ActionResult Index(int? raceId)
        {
            if (raceId.HasValue)
                ViewBag.RaceId = raceId.Value;
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Classes = AthleteClassModel.GetAll();
            RaceAthleteViewModel model = new RaceAthleteViewModel()
            {
                AthletesAvailable = new List<AthleteModel>(),
                AthletesConnected = new List<AthleteModel>()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RaceAthleteViewModel model, string changeRace, string changeClass, string add, string remove, string ddlRaces, string ddlClasses)
        {
            ModelState.Clear();
            ViewBag.Controller = "";
            ViewBag.Action = "";
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Classes = AthleteClassModel.GetAll();
            RestoreSavedState(model);
            if (!string.IsNullOrEmpty(add))
                AssignAthleteToRace(model);
            else if (!string.IsNullOrEmpty(remove))
                RemoveAthleteFromRace(model);
            else if (!string.IsNullOrEmpty(changeRace))
            {
                model.RaceId = Convert.ToInt32(ddlRaces);
                var race = RaceModel.GetById(model.RaceId);
                model.AthletesAvailable = new List<AthleteModel>();
                model.AthletesConnected = race.GetAthletes().OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
            }
            else if (!string.IsNullOrEmpty(changeClass))
            {
                var race = RaceModel.GetById(model.RaceId);
                var classid = 0;
                if (int.TryParse(ddlClasses, out classid))
                    model.ClassId = classid;
                model.AthletesConnected = race.GetAthletes().OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
                model.AthletesAvailable = race.GetAthletesNotConnected(model.ClassId).OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
            }
            SaveState(model);
            return View(model);
        }

        private void ChangeClass(RaceAthleteViewModel model)
        {
            throw new NotImplementedException();
        }

        private void SaveState(RaceAthleteViewModel model)
        {
            model.SavedConnected = string.Join(",", model.AthletesConnected.Select(a => a.Id.ToString()).ToArray());
        }

        private void RestoreSavedState(RaceAthleteViewModel model)
        {
            model.AthletesConnected = new List<AthleteModel>();

            if (!string.IsNullOrEmpty(model.SavedConnected))
            {
                string[] athleteid = model.SavedConnected.Split(',');
                var athletes = AthleteModel.GetAll().Where(a => athleteid.Contains(a.Id.ToString()));
                model.AthletesConnected.AddRange(athletes);
            }
        }

        void AssignAthleteToRace(RaceAthleteViewModel model)
        {
            var race = RaceModel.GetById(model.RaceId);

            if (model.AvailableSelected != null)
            {
                var athletes = AthleteModel.GetAll().Where(a => model.AvailableSelected.Contains(a.Id));
                foreach (var athlete in athletes)
                {
                    athlete.ConnectToRace(model.RaceId);
                }
                model.AthletesConnected = race.GetAthletes();
                model.AthletesAvailable = race.GetAthletesNotConnected(model.ClassId);
                model.AvailableSelected = null;
            }
        }

        void RemoveAthleteFromRace(RaceAthleteViewModel model)
        {
            var race = RaceModel.GetById(model.RaceId);
            if (model.ConnectedSelected != null)
            {
                var athletes = AthleteModel.GetAll().Where(a => model.ConnectedSelected.Contains(a.Id));
                foreach (var athlete in athletes)
                {
                    athlete.RemoveFromRace(model.RaceId);
                }
                model.AthletesConnected = race.GetAthletes();
                model.AthletesAvailable = race.GetAthletesNotConnected(model.ClassId);
                model.ConnectedSelected = null;
            }
        }

    }
}
