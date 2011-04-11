using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            ViewBag.Feedback = "";
            return View();
        }

        //
        // POST: /Event/Create

        [HttpPost]
        public ActionResult Create(EventModel model)
        {
            try
            {
                var newModel = new EventModel(model.Name, model.EventDate);
                newModel.Save();
                ViewData.ModelState.Clear();
                ViewBag.Feedback = "Stevne ble opprettet";
                return View();
            }
            catch
            {
                ViewBag.Feedback = "Det skjedde en feil under lagring av stevne";
                return View();
            }
        }
    }
}
