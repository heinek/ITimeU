using System.Linq;
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
        /// <param name="checkpoint_id">The checkpoint_id.</param>
        [HttpGet]
        public ActionResult Index(int? checkpoint_id)
        {
            var timer = new TimerModel();

            if (Session["timer"] == null)
                Session["timer"] = timer;
            else
                timer = (TimerModel)Session["timer"];

            if (checkpoint_id != null)
            {
                CheckpointModel checkpoint = CheckpointModel.getById((int)checkpoint_id);
                ViewData["checkpoint"] = checkpoint.Name;
            }
            else
                ViewData["checkpoint"] = "Ingen valgt";

            return View("Index", timer);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public ActionResult Start()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            timerModel.Start();
            Session["timer"] = timerModel;

            return View("Index", timerModel);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public ActionResult Stop()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            timerModel.Stop();
            Session["timer"] = timerModel;
            return View("Index", timerModel);
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
        public ActionResult SaveRuntime(string runtime)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int milliseconds;
            int.TryParse(runtime, out milliseconds);
            timerModel.AddRuntime(milliseconds);
            return Content(timerModel.RuntimeDic.ToListboxvalues(true));
        }

        /// <summary>
        /// Edits the runtime.
        /// </summary>
        /// <param name="orginalruntimeid">The orginalruntimeid.</param>
        /// <param name="newruntime">The newruntime.</param>
        public ActionResult EditRuntime(string orginalruntimeid, string newruntime)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int orgid, milliseconds;
            int.TryParse(orginalruntimeid.Trim(), out orgid);
            int.TryParse(newruntime.Trim(), out milliseconds);
            //RuntimeModel runtimeModel = timerModel.Runtimes.Where(runtime => runtime.Id == orgid).Single();
            timerModel.EditRuntime(orgid, milliseconds);
            return Content(timerModel.RuntimeDic.ToListboxvalues(true));
        }

        /// <summary>
        /// Deletes the runtime.
        /// </summary>
        /// <param name="runtimeid">The runtimeid.</param>
        public ActionResult DeleteRuntime(string runtimeid)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int rtid;
            int.TryParse(runtimeid.Trim(), out rtid);
            timerModel.DeleteRuntime(rtid);
            return Content(timerModel.RuntimeDic.ToListboxvalues(true));
        }

        public int GetLastRuntimeId()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            return timerModel.RuntimeDic.Last().Key;
        }
    }
}
