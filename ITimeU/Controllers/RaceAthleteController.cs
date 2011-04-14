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
        public ActionResult Index()
        {
            ViewBag.Races = RaceModel.GetRaces();
            RaceAthleteViewModel model = new RaceAthleteViewModel()
            {
                AthletesAvailable = new List<AthleteModel>(),
                AthletesConnected = new List<AthleteModel>()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RaceAthleteViewModel model, string changeRace, string add, string remove, string ddlRaces)
        {
            ModelState.Clear();
            ViewBag.Races = RaceModel.GetRaces();
            RestoreSavedState(model);
            if (!string.IsNullOrEmpty(add))
                AssignAthleteToRace(model);
            else if (!string.IsNullOrEmpty(remove))
                RemoveAthleteFromRace(model);
            else if (!string.IsNullOrEmpty(changeRace))
            {
                //model.AthletesAvailable = AthleteModel.GetAll().Except(model.AthletesConnected).ToList();
                model.RaceId = Convert.ToInt32(ddlRaces);
                var race = RaceModel.GetById(model.RaceId);
                model.AthletesConnected = race.GetAthletes().OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();
                model.AthletesAvailable = race.GetAthletesNotConnected().OrderBy(athlete => athlete.FirstName).ThenBy(athlete => athlete.LastName).ToList();

            }
            SaveState(model);
            return View(model);
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
                model.AthletesAvailable = race.GetAthletesNotConnected();
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
                model.AthletesAvailable = race.GetAthletesNotConnected();
                model.ConnectedSelected = null;
            }
        }

    }
}
