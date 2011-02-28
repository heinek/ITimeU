using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class TimerController : Controller
    {

        //
        // GET: /Timer/

        /// <summary>
        /// Indexes the specified checkpoint_id.
        /// </summary>
        /// <param name="Id">The timer id.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int Id)
        {
            Logging.LogWriter.getInstance().Write("yoyoyo");
            var timer = new TimerModel(Id);
            ViewBag.Checkpoints = timer.GetCheckpoints();
            Session["timer"] = timer;
                checkpoint.Timer = timer;
            return View("Index", timer);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public ActionResult Start()
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.Start();
            //ViewBag.Checkpoints = timerModel.GetCheckpoints();
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
            Session["timer"] = timer;
            return View("Index", timer);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public ActionResult Restart()
        {
            // TODO: Update javascript to call start instead of restart.
            return Start();
        }
        /// <summary>
        /// Saves the runtime.
        /// </summary>
        /// <param name="runtime">The runtime.</param>
        public ActionResult SaveRuntime(string runtime, string checkpointid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            int milliseconds, cpid;
            int.TryParse(runtime, out milliseconds);
            int.TryParse(checkpointid, out cpid);
            timer.AddRuntime(milliseconds, cpid);
            Session["timer"] = timer;
            return Content(timer.CheckpointRuntimes[timer.CheckpointId].ToListboxvalues(true, true));
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
        public ActionResult EditRuntime(string orginalruntimeid, string hour, string min, string sek, string msek)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            int orgid, h, m, s, ms;
            int.TryParse(orginalruntimeid.Trim(), out orgid);
            int.TryParse(hour, out h);
            int.TryParse(min, out m);
            int.TryParse(sek, out s);
            int.TryParse(msek, out ms);
            timer.EditRuntime(orgid, h, m, s, ms);
            Session["timer"] = timer;
            return Content(timer.CheckpointRuntimes[timer.CheckpointId].ToListboxvalues(true, true));
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public ActionResult DeleteRuntime(string runtimeid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            int rtid;
            int.TryParse(runtimeid.Trim(), out rtid);
            timer.DeleteRuntime(rtid);
            Session["timer"] = timer;
            return Content(timer.CheckpointRuntimes[timer.CheckpointId].ToListboxvalues(true, true));
        }

        public ActionResult ChangeCheckpointId(int checkpointid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.ChangeCheckpoint(checkpointid);
            Session["timer"] = timer;
            return Content(timer.CheckpointRuntimes[timer.CheckpointId].ToListboxvalues(true, true));
        }
    }
}
