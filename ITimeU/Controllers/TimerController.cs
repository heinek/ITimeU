using System;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class TimerController : Controller
    {
        /// <summary>
        /// Indexes the specified checkpoint_id.
        /// </summary>
        /// <param name="Id">The race id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int id)
        {
            var race = RaceModel.GetById(id);

            TimerModel timer;
            if (race.GetTimerId().HasValue)
                timer = TimerModel.GetTimerById(race.GetTimerId().Value);
            else
            {
                timer = new TimerModel();
                timer.RaceID = id;
            }
            timer.SaveToDb();
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(id);
            ViewBag.RaceId = id;
            Session["timer"] = timer;
            return View("Index", timer);
        }

        [HttpGet]
        public ActionResult Testing()
        {
            return View("Testing");
        }
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public ActionResult Start()
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.Start();
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(timer.RaceID.Value);
            Session["timer"] = timer;
            return View("Index", timer);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public ActionResult Stop()
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.Stop();
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(timer.RaceID.Value);
            Session["timer"] = timer;
            return View("Index", timer);
        }

        /// <summary>
        /// Saves the runtime.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        public ActionResult SaveRuntime(int runtime, int checkpointid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            var runtimeModel = timer.AddRuntime(runtime, checkpointid);
            TimeMergerModel.Merge(checkpointid);
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="orginalruntimeid">The orginalruntimeid.</param>
        /// <param name="hour">Hours.</param>
        /// <param name="min">Minutes.</param>
        /// <param name="sek">Seconds.</param>
        /// <param name="msek">Milliseconds.</param>
        /// <returns></returns>
        public ActionResult EditRuntime(int orginalruntimeid, int hour, int min, int sek, int msek)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.EditRuntime(orginalruntimeid, hour, min, sek, msek);
            var runtime = RuntimeModel.getById(orginalruntimeid);
            TimeMergerModel.Merge(runtime.CheckPointId);
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public ActionResult DeleteRuntime(int runtimeid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            var runtime = RuntimeModel.getById(runtimeid);
            timer.DeleteRuntime(runtimeid);
            TimeMergerModel.Merge(runtime.CheckPointId);
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        /// <summary>
        /// Changes the checkpoint.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeCheckpoint(int checkpointid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.ChangeCheckpoint(checkpointid);
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        private string SaveToSessionAndReturnRuntimes(TimerModel timer)
        {
            var runtimeDic = timer.CheckpointRuntimes[timer.CurrentCheckpointId].ToListboxvalues(sorting: ExtensionMethods.ListboxSorting.Descending, toTimer: true);
            Session["timer"] = timer;
            return runtimeDic;
        }

        [HttpGet]
        public ActionResult Speaker(int id)
        {
            ViewBag.RaceId = id;
            var race = RaceModel.GetById(id);
            TimerModel timer = null;
            if (race.GetTimerId().HasValue)
                timer = TimerModel.GetTimerById(race.GetTimerId().Value);
            else
            {
                timer = new TimerModel();
                timer.RaceID = id;
            }
            timer.SaveToDb();
            ViewBag.RaceId = id;
            Session["timer"] = timer;
            var raceintermediates = RaceIntermediateModel.GetRaceintermediatesForRace(id).
                Select(raceintermediate => new ResultsViewModel()
                {
                    Checkpointname = raceintermediate.CheckpointModel.Name,
                    Clubname = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteModel.Club.Name : " - ",
                    Fullname = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteModel.FullName : " - ",
                    Startnumber = raceintermediate.AthleteId.HasValue ? (raceintermediate.AthleteModel.StartNumber.HasValue ? raceintermediate.AthleteModel.StartNumber.Value : 0) : raceintermediate.CheckpointorderModel.StartingNumber,
                    Time = raceintermediate.RuntimeModel.RuntimeToTime
                });
            return View("Speaker", raceintermediates);
        }

        [HttpGet]
        public ActionResult GetStartruntimeForSpeaker(int raceid)
        {
            var race = RaceModel.GetById(raceid);

            TimerModel timer = null;
            if (race.GetTimerId().HasValue)
                timer = TimerModel.GetTimerById(race.GetTimerId().Value);
            DateTime starttime;
            int runtime = 0;

            if (timer.StartTime.HasValue)
            {
                starttime = timer.StartTime.Value;
                var ts = DateTime.Now - starttime;
                runtime = (int)ts.TotalMilliseconds;
            }
            return Content(runtime.ToString());
        }

        public ActionResult Update(int id)
        {
            var raceintermediates = RaceIntermediateModel.GetRaceintermediatesForRace(id).
                Select(raceintermediate => new ResultsViewModel()
                {
                    Checkpointname = raceintermediate.CheckpointModel.Name,
                    Clubname = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteModel.Club.Name : " - ",
                    Fullname = raceintermediate.AthleteId.HasValue ? raceintermediate.AthleteModel.FullName : " - ",
                    Startnumber = raceintermediate.AthleteId.HasValue ? (raceintermediate.AthleteModel.StartNumber.HasValue ? raceintermediate.AthleteModel.StartNumber.Value : 0) : raceintermediate.CheckpointorderModel.StartingNumber,
                    Time = raceintermediate.RuntimeModel.RuntimeToTime
                }).ToList();
            return Content(raceintermediates.ToTable());
        }

        [HttpGet]
        public ActionResult GetStartruntime()
        {
            var timer = (TimerModel)Session["timer"];
            DateTime starttime;
            int runtime = 0;

            if (timer.StartTime.HasValue && timer.IsStarted)
            {
                starttime = timer.StartTime.Value;
                var ts = DateTime.Now - starttime;
                runtime = (int)ts.TotalMilliseconds;
            }
            return Content(runtime.ToString());
        }

        [HttpPost]
        public ActionResult ResetRace(int raceid)
        {
            RaceIntermediateModel.DeleteRaceintermediatesForRace(raceid);
            var timer = (TimerModel)Session["timer"];
            foreach (var key in timer.CheckpointRuntimes.Keys)
            {
                timer.CheckpointRuntimes[key].Clear();
            }
            Session["timer"] = timer;

            return Content("");
        }
    }
}
