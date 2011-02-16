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
        public ActionResult Index()
        {
            var timer = new TimerModel();

            if (Session["timer"] == null)
            {
                Session["timer"] = timer;
            }
            else
            {
                timer = (TimerModel)Session["timer"];
            }
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

        public ActionResult EditRuntime(string runtimeid, string newruntime)
        {
            TimerModel timerModel = (TimerModel)Session["timer"];
            int rtid, milliseconds;
            int.TryParse(runtimeid, out rtid);
            int.TryParse(newruntime, out milliseconds);
            RuntimeModel runtimeModel = RuntimeModel.getById(rtid);
            timerModel.EditRuntime(runtimeModel, milliseconds);
            return Content(newruntime);

        }
    }
}
