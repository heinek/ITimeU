using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class TimerController : Controller
    {
        //
        // GET: /Timer/

        public ActionResult Index(TimerModel timerModel)
        {
            if (timerModel == null)
            {
                timerModel = new TimerModel();
            }
            if (timerModel.IsStarted)
            {
                ViewBag.StartTime = timerModel.StartTime;
            }
            ViewBag.IsStarted = timerModel.IsStarted;
            return View("Index", timerModel);
        }

        public ActionResult StartTimer(TimerModel timerModel)
        {
            timerModel.Start();
            ViewBag.IsStarted = timerModel.IsStarted;
            ViewBag.StartTime = timerModel.StartTime;
            return View("Index", timerModel);
        }

    }
}
