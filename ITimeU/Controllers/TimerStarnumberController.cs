using System;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class TimeStartnumberController : Controller
    {
        //
        // GET: /TimeStartnumber/

        /// <summary>
        /// Returns the index view
        /// </summary>
        /// <param name="raceId">The race id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int raceId)
        {

            var race = RaceModel.GetById(raceId);

            TimerModel timer;
            if (race.GetTimerId().HasValue)
                timer = TimerModel.GetTimerById(race.GetTimerId().Value);
            else
            {
                timer = new TimerModel();
                timer.RaceID = raceId;
            }
            timer.SaveToDb();

            TimeStartnumberModel timeStartnumberModel;
            timeStartnumberModel = new TimeStartnumberModel(timer);
            var checkpointOrder = new CheckpointOrderModel();

            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(raceId);
            ViewBag.RaceId = raceId;
            timeStartnumberModel.ChangeCheckpoint(timer.GetFirstCheckpointId());
            timeStartnumberModel.CheckpointOrder = checkpointOrder;
            Session["TimeStartnumber"] = timeStartnumberModel;
            return View("Index", timeStartnumberModel);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public ActionResult Start()
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.Timer.Start();
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(timeStartnumberModel.Timer.RaceID.Value);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content("");
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public ActionResult Stop()
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.Timer.Stop();
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(timeStartnumberModel.Timer.RaceID.Value);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content("");
        }

        /// <summary>
        /// Adds the startnumber.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <param name="startnumber">The startnumber.</param>
        /// <param name="runtime">The runtime.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStartnumber(int checkpointId, int startnumber, int runtime)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.AddStartnumber(checkpointId, startnumber, runtime);
            Session["TimeStartnumber"] = timeStartnumberModel;
            TimeMergerModel.Merge(checkpointId);
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());
        }

        /// <summary>
        /// Changes the checkpoint.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeCheckpoint(int checkpointid)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.Timer.ChangeCheckpoint(checkpointid);
            timeStartnumberModel.ChangeCheckpoint(checkpointid);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());
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
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            var runtimeId = RaceIntermediateModel.GetRaceintermediate(checkpointid, checkpointOrderId).RuntimeId;
            RuntimeModel.EditRuntime(runtimeId, hour, min, sek, msek);
            timeStartnumberModel.EditStartnumber(checkpointid, checkpointOrderId, startnumber);
            Session["TimeStartnumber"] = timeStartnumberModel;
            TimeMergerModel.Merge(checkpointid);
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());
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
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.DeleteRaceintermediate(checkpointid, checkpointOrderId);
            Session["TimeStartnumber"] = timeStartnumberModel;
            TimeMergerModel.Merge(checkpointid);
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());

        }

        [HttpGet]
        public ActionResult GetStartruntime()
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            DateTime starttime;
            int runtime = 0;

            if (timeStartnumberModel.Timer.StartTime.HasValue && timeStartnumberModel.Timer.IsStarted)
            {
                starttime = timeStartnumberModel.Timer.StartTime.Value;
                var ts = DateTime.Now - starttime;
                runtime = (int)ts.TotalMilliseconds;
            }
            return Content(runtime.ToString());
        }
        [HttpPost]
        public ActionResult ResetRace(int raceid)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            foreach (var key in timeStartnumberModel.CheckpointIntermediates.Keys)
            {
                timeStartnumberModel.CheckpointIntermediates[key].Clear();
            }
            RaceIntermediateModel.DeleteRaceintermediatesForRace(raceid);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content("");
        }
    }
}
