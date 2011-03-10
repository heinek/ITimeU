using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;
using ITimeU.Tests.Models;

namespace ITimeU.Controllers
{
    public class TimeMergerController : Controller
    {
        //
        // GET: /TimeMerger/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Races = RaceModel.GetRaces();
            ViewBag.Timers = new List<Timer>();
            ViewBag.Checkpoints = new List<Checkpoint>();
            var dicStartnumbers = new Dictionary<int, int>();
            ViewBag.Startnumbers = dicStartnumbers;
            var dicTimestamps = new Dictionary<int, int>();
            ViewBag.Timestamps = dicTimestamps;
            var timeMergeModel = new TimeMergerModel();
            var dicMergedlist = new Dictionary<int, int>();
            ViewBag.Mergedlist = dicMergedlist;
            return View(timeMergeModel);
        }

        [HttpGet]
        public ActionResult Testing()
        {
            return View("Testing");
        }

        [HttpPost]
        public ActionResult GetTimers(int raceId)
        {
            var timeMergeModel = new TimeMergerModel();
            timeMergeModel.Timers = TimerModel.GetTimers(raceId);
            var dic = timeMergeModel.Timers.ToDictionary(timer => timer.TimerID, timer => timer.StartTime);
            return Content(dic.ToListboxvalues());
        }

        [HttpPost]
        public ActionResult GetCheckpoints(int timerId)
        {
            var timerModel = new TimerModel(timerId);
            var dic = timerModel.GetCheckpoints().ToDictionary(checkpoint => checkpoint.CheckpointID, checkpoint => checkpoint.Name);
            return Content(dic.ToListboxvalues());
        }

        [HttpPost]
        public ActionResult GetTimestamps(int checkpointId)
        {
            return Content(RuntimeModel.GetRuntimes(checkpointId).ToListboxvalues(sorting: ExtensionMethods.ListboxSorting.Ascending, toTimer: true));
        }

        [HttpPost]
        public ActionResult GetTimestampCount(int checkpointId)
        {
            return Content(RuntimeModel.GetRuntimes(checkpointId).Count.ToString());
        }

        [HttpPost]
        public ActionResult GetStartnumbers(int checkpointId)
        {
            return Content(CheckpointOrderModel.GetCheckpointOrders(checkpointId).ToListboxvalues());
        }

        [HttpPost]
        public ActionResult GetStartnumbersCount(int checkpointId)
        {
            return Content(CheckpointOrderModel.GetCheckpointOrders(checkpointId).Count.ToString());
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <param name="orginalruntimeid">The orginalruntimeid.</param>
        /// <param name="hour">Hours.</param>
        /// <param name="min">Minutes.</param>
        /// <param name="sek">Seconds.</param>
        /// <param name="msek">Milliseconds.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRuntime(string checkpointId, string orginalruntimeid, string hour, string min, string sek, string msek)
        {
            int cpid, orgid, h, m, s, ms;
            int.TryParse(checkpointId, out cpid);
            int.TryParse(orginalruntimeid.Trim(), out orgid);
            int.TryParse(hour, out h);
            int.TryParse(min, out m);
            int.TryParse(sek, out s);
            int.TryParse(msek, out ms);
            RuntimeModel.EditRuntime(orgid, h, m, s, ms);
            return Content(RuntimeModel.GetRuntimes(cpid).ToListboxvalues(sorting: ExtensionMethods.ListboxSorting.Ascending, toTimer: true));
        }

        //[HttpPost]
        //public ActionResult Merge(int checkpointId, string timestampData, string startnumberData)
        //{
        //    return Content(TimeMergerModel.Merge(checkpointId, timestampData, startnumberData).ToString());
        //}

        [HttpPost]
        public ActionResult Merge(int checkpointId)
        {
            TimeMergerModel.Merge(checkpointId, RuntimeModel.GetRuntimes(checkpointId), CheckpointOrderModel.GetCheckpointOrders(checkpointId));
            return Content(TimeMergerModel.GetMergedList(checkpointId).ToListboxvalues());
        }

        [HttpPost]
        public ActionResult DeleteCheckpointOrder(int checkpointId, int checkpointOrdreId)
        {
            CheckpointOrderModel.DeleteCheckpointOrder(checkpointOrdreId);
            return Content(CheckpointOrderModel.GetCheckpointOrders(checkpointId).ToListboxvalues());
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        [HttpPost]
        public ActionResult DeleteRuntime(int checkpointId, string runtimeid)
        {
            int rtid;
            int.TryParse(runtimeid.Trim(), out rtid);
            RuntimeModel.DeleteRuntime(rtid);
            return Content(RuntimeModel.GetRuntimes(checkpointId).ToListboxvalues(sorting: ExtensionMethods.ListboxSorting.Ascending, toTimer: true));
        }
    }
}
