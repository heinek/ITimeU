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
            TimerModel timer = null;
            if (race.HasTimer())
                timer = new TimerModel(race.GetTimerId());
            else
            {
                timer = new TimerModel();
                timer.RaceID = id;
            }
            timer.SaveToDb();
            ViewBag.Checkpoints = timer.GetCheckpoints();
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
            ViewBag.Checkpoints = timer.GetCheckpoints();
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
            ViewBag.Checkpoints = timer.GetCheckpoints();
            Session["timer"] = timer;
            return View("Index", timer);
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
    }
}
