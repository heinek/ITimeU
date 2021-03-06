﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;


namespace ITimeU.Controllers
{
    public class ResultsController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Events = EventModel.GetEvents();
            ViewBag.Races = new List<RaceModel>();
            ViewBag.Checkpoints = new List<Checkpoint>();
            var raceIntermediates = new List<RaceIntermediateModel>();
            ViewBag.Intermediates = new Dictionary<int, int>();
            return View("Index", raceIntermediates);
        }

        /// <summary>
        /// Gets the timers.
        /// </summary>
        /// <param name="raceId">The race id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTimers(int raceId)
        {
            return Content(TimerModel.GetTimers(raceId).ToDictionary(timer => timer.TimerID, timer => timer.StartTime).ToListboxvalues());
        }

        /// <summary>
        /// Gets the races.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRaces(int eventId)
        {
            return Content(RaceModel.GetRaces(eventId).ToDictionary(race => race.RaceId, race => race.Name).ToListboxvalues());
        }

        /// <summary>
        /// Gets the checkpoints.
        /// </summary>
        /// <param name="raceId">The race id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCheckpoints(int raceId)
        {
            return Content(CheckpointModel.GetCheckpoints(raceId).ToDictionary(checkpoint => checkpoint.Id, checkpoint => checkpoint.Name).ToListboxvalues());
        }

        /// <summary>
        /// Gets the raceintermediates.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRaceintermediates(int checkpointId)
        {
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Timers = new List<Timer>();
            ViewBag.Checkpoints = new List<Checkpoint>();
            ViewBag.Intermediates = new Dictionary<int, int>();
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointId);
            var listboxvalues = raceIntermediates.
                OrderBy(raceintermediate => raceintermediate.RuntimeModel.Runtime).
                ToList().
                ToListboxvalues();
            return Content(listboxvalues);
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="checkpointid">The orginalruntimeid.</param>
        /// <param name="hour">Hours.</param>
        /// <param name="min">Minutes.</param>
        /// <param name="sek">Seconds.</param>
        /// <param name="msek">Milliseconds.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRuntime(int checkpointid, int checkpointOrderId, int hour, int min, int sek, int msek, int startnumber)
        {
            var raceintermediate = RaceIntermediateModel.GetRaceintermediate(checkpointid, checkpointOrderId);
            RuntimeModel.EditRuntime(raceintermediate.RuntimeId, hour, min, sek, msek);
            CheckpointOrderModel.EditCheckpointOrder(raceintermediate.CheckpointOrderID, startnumber);
            return Content(RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid).OrderBy(raceinter => raceinter.RuntimeModel.Runtime).ToList().ToListboxvalues());
        }

        /// <summary>
        /// Deletes the raceintermediate.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <param name="checkpointOrderId">The checkpoint order id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRaceintermediate(int checkpointid, int checkpointOrderId)
        {
            RaceIntermediateModel.DeleteRaceintermediate(checkpointid, checkpointOrderId);
            return Content(RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid).ToListboxvalues());
        }

        /// <summary>
        /// Approves the results.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Approve(int checkpointid)
        {
            var checkpoint = CheckpointModel.getById(checkpointid);
            RaceIntermediateModel.MergeAthletes(checkpoint.RaceId.Value);
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid);
            return Content(raceIntermediates.ToTable());
        }

        /// <summary>
        /// Prints the results.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <param name="racename">The racename.</param>
        /// <param name="checkpointname">The checkpointname.</param>
        /// <returns></returns>
        public ActionResult Print(int checkpointid)
        {
            var checkpoint = CheckpointModel.getById(checkpointid);
            ViewBag.RaceName = checkpoint.Race.Name;
            ViewBag.CheckpointName = checkpoint.Name;
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid);
            return View(raceIntermediates);
        }

        [HttpGet]
        public ActionResult ResultsSpeaker(int raceId)
        {
            return View();
        }

        public ActionResult ResultsForSpeaker(int checkpointId)
        {
            ViewBag.CheckpointId = checkpointId;
            ViewBag.RaceId = CheckpointModel.getById(checkpointId).RaceId;
            return View(RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointId));
        }

        public ActionResult ResultSetup()
        {
            return View();
        }

        public ActionResult SelectEvent()
        {
            ViewBag.Events = EventModel.GetEvents();
            return View("SelectEvent");
        }

        public ActionResult SelectRace(int eventId)
        {
            ViewBag.Races = RaceModel.GetRaces(eventId);
            return View("SelectRace");
        }

        public ActionResult SelectCheckpoint(int raceId)
        {
            ViewBag.EventId = RaceModel.GetById(raceId).EventId;
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(raceId);
            return View("SelectCheckpoint");
        }
    }
}
