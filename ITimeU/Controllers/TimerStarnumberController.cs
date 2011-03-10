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
            timeStartnumberModel.CurrentCheckpointId = timer.GetFirstCheckpointId();
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

        [HttpPost]
        public ActionResult ChangeCheckpoint(int checkpointid)
        {
            var timeStartnumberModel = (TimeStartnumberModel)Session["TimeStartnumber"];
            timeStartnumberModel.Timer.ChangeCheckpoint(checkpointid);
            Session["TimeStartnumber"] = timeStartnumberModel;
            return Content(timeStartnumberModel.CheckpointIntermediates[timeStartnumberModel.CurrentCheckpointId].ToListboxvalues());
        }
    }
}
