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
        /// <param name="timerId">The timer id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int timerId)
        {
            var timer = new TimerModel(timerId);
            var checkpointOrder = new CheckpointOrderModel();

            ViewBag.Checkpoints = timer.GetCheckpoints();
            var timeStartnumberModel = new TimeStartnumberModel(timer);
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
            ViewBag.Checkpoints = timeStartnumberModel.Timer.GetCheckpoints();
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
            ViewBag.Checkpoints = timeStartnumberModel.Timer.GetCheckpoints();
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
        public ActionResult AddStartnumber(string checkpointId, string startnumber, string runtime)
        {
            int cpId, startNr, runtimeint;
            int.TryParse(checkpointId, out cpId);
            int.TryParse(startnumber, out startNr);
            int.TryParse(runtime, out runtimeint);
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.AddStartnumber(cpId, startNr, runtimeint);
            Session["TimeStartnumber"] = timeStartnumberModel;
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
        public ActionResult EditRuntime(string checkpointid, string checkpointOrderId, string hour, string min, string sek, string msek, string startnumber)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            int cpid, cporderid, h, m, s, ms, startnum;
            int.TryParse(checkpointid.Trim(), out cpid);
            int.TryParse(checkpointOrderId.Trim(), out cporderid);
            int.TryParse(hour, out h);
            int.TryParse(min, out m);
            int.TryParse(sek, out s);
            int.TryParse(msek, out ms);
            int.TryParse(startnumber, out startnum);

            var runtimeId = RaceIntermediateModel.GetRaceintermediate(cpid, cporderid).RuntimeId;
            timeStartnumberModel.EditRuntime(runtimeId, h, m, s, ms);
            timeStartnumberModel.EditStartnumber(cpid, cporderid, startnum);

            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());
        }

        /// <summary>
        /// Deletes the raceintermediate.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <param name="checkpointOrderId">The checkpoint order id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRaceintermediate(string checkpointid, string checkpointOrderId)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            int cpid, cporderid;
            int.TryParse(checkpointid.Trim(), out cpid);
            int.TryParse(checkpointOrderId.Trim(), out cporderid);
            timeStartnumberModel.DeleteRaceintermediate(cpid, cporderid);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());

        }
    }
}
