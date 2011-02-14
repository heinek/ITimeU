using System.Web.Mvc;
using ITimeU.Models;
using ITimeU.Logging;

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

    }
}
