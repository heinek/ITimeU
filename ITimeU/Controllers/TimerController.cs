using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;
using ITimeU.Tests.Models;

namespace ITimeU.Controllers
{
    public class TimerController : Controller
    {
        //
        // GET: /Timer/

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

        public ActionResult Start()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            timerModel.Start();
            Session["timer"] = timerModel;

            return View("Index", timerModel);
        }

        public ActionResult Stop()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            timerModel.Stop();
            Session["timer"] = timerModel;
            return View("Index", timerModel);
        }

        public ActionResult Reset()
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            timerModel.Reset();
            Session["timer"] = timerModel;
            return View("Index", timerModel);
        }
        public ActionResult SaveRuntime(string runtime)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int milliseconds;
            int.TryParse(runtime, out milliseconds);
            timerModel.AddRuntime(milliseconds);
            return Content(runtime);
        }

        public ActionResult EditRuntime(string orginalruntime, string newruntime)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int orgmilliseconds, milliseconds;
            int.TryParse(orginalruntime.Trim(), out orgmilliseconds);
            int.TryParse(newruntime.Trim(), out milliseconds);
            RuntimeModel runtimeModel = timerModel.Runtimes.OrderByDescending(runtime => runtime.Id).Where(runtime => runtime.Runtime == orgmilliseconds).First();
            timerModel.EditRuntime(runtimeModel, milliseconds);
            return Content(newruntime);

        }
    }
}
