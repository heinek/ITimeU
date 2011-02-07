using System.Web.Mvc;
using ITimeU.Models;

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

        //[HttpPost]
        //public ActionResult Index(TimerModel timerModel)
        //{
        //    timerModel = (TimerModel)Session["timer"];
        //    timerModel.Start();
        //    Session["timer"] = timerModel;

        //    return View("Index", timerModel);
        //}

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
    }
}
