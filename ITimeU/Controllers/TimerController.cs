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
            var timer = new TimerModel(Id);
            ViewBag.Checkpoints = timer.GetCheckpoints();
            Session["timer"] = timer;
            return View("Index", timer);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public ActionResult Start()
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.Start();
            Session["timer"] = timer;
            ViewBag.Checkpoints = timer.GetCheckpoints();
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
            ViewBag.Checkpoints = timer.GetCheckpoints();
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
            return Content(SaveToSessionAndReturnRuntimes(timer));
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
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        /// <summary>
        /// Changes the checkpoint.
        /// </summary>
        /// <param name="checkpointid">The checkpointid.</param>
        /// <returns></returns>
        public ActionResult ChangeCheckpoint(int checkpointid)
        {
            TimerModel timer = (TimerModel)Session["timer"];
            timer.ChangeCheckpoint(checkpointid);
            return Content(SaveToSessionAndReturnRuntimes(timer));
        }

        private string SaveToSessionAndReturnRuntimes(TimerModel timer)
        {
            var runtimeDic = timer.CheckpointRuntimes[timer.CurrentCheckpointId].ToListboxvalues(true, true);
            Session["timer"] = timer;
            return runtimeDic;
        }
    }
}
