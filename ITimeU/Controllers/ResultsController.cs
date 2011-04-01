using System.Collections.Generic;
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
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Timers = new List<Timer>();
            ViewBag.Checkpoints = new List<Checkpoint>();
            var raceIntermediates = new List<RaceIntermediateModel>();
            ViewBag.Intermediates = new Dictionary<int, int>();
            return View("Index", raceIntermediates);
        }

        [HttpPost]
        public ActionResult GetTimers(int raceId)
        {
            return Content(TimerModel.GetTimers(raceId).ToDictionary(timer => timer.TimerID, timer => timer.StartTime).ToListboxvalues());
        }

        [HttpPost]
        public ActionResult GetCheckpoints(int timerId)
        {
            var timerModel = new TimerModel(timerId);
            var dic = timerModel.GetCheckpoints().ToDictionary(checkpoint => checkpoint.CheckpointID, checkpoint => checkpoint.Name);
            return Content(dic.ToListboxvalues());
        }

        [HttpPost]
        public ActionResult GetRaceintermediates(int checkpointId)
        {
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Timers = new List<Timer>();
            ViewBag.Checkpoints = new List<Checkpoint>();
            ViewBag.Intermediates = new Dictionary<int, int>();
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointId);
            var listboxvalues = raceIntermediates.OrderBy(raceintermediate => raceintermediate.RuntimeModel.Runtime).ToList().ToListboxvalues();
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
            return Content(RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid).ToListboxvalues());
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

        [HttpPost]
        public ActionResult Approve(int checkpointid)
        {
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid);
            return Content(raceIntermediates.ToTable());
        }

        public ActionResult Print(int checkpointid, string racename, string checkpointname)
        {
            ViewBag.RaceName = racename;
            ViewBag.CheckpointName = checkpointname;
            var raceIntermediates = RaceIntermediateModel.GetRaceintermediatesForCheckpoint(checkpointid);
            return View(raceIntermediates);
        }
    }
}
