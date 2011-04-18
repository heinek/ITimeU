using System;
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
            return View(EventModel.GetEvents());
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
            if (model.EventDate < DateTime.Today)
            {
                ViewBag.Error = "Dato kan ikke være mindre enn dagens dato";
                return View();
            }
            if (EventModel.EventNameExists(model.Name))
            {
                ViewBag.Error = "Et stevne med samme navn eksisterer allerede";
                return View();
            }
            var newModel = new EventModel(model.Name, model.EventDate);
            if (newModel.Save())
            {
                ViewData.ModelState.Clear();
                ViewBag.Success = "Stevne ble opprettet";
                return View();
            }
            else
            {
                ViewBag.Error = "Det skjedde en feil under lagring av stevne";
                return View();
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var eventToDelete = EventModel.GetById(id);
            eventToDelete.Delete();
            return View("Index", EventModel.GetEvents());
        }
    }
}
