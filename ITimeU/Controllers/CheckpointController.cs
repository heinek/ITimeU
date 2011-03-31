using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class CheckpointController : Controller
    {
        //
        // GET: /Checkpoint/

        public ActionResult Index(int? id)
        {
            if (id != null)
                return RedirectToAction("Index", "Timer", new { checkpoint_id = id });

            return View(CheckpointModel.getAll());
        }

        public ActionResult Create()
        {
            return View("Create", RaceModel.GetRaces());
        }

        [HttpPost]
        public ActionResult Create(int raceId, string txtCheckpointName)
        {
            CheckpointModel model = new CheckpointModel(txtCheckpointName, raceId); // race with raceId exists in the database already.
            model.SaveToDb();
            return RedirectToAction("Create"); // Redirect in order to reset form values.
        }

    }
}

