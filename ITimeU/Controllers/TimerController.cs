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

    }
}
