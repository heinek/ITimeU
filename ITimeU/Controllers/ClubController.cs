using System;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class ClubController : Controller
    {
        //
        // GET: /Club/

        public ActionResult Index()
        {
            return View(ClubModel.GetClubs());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var club = ClubModel.GetById(id);
            return View("Edit", club);
        }

        [HttpPost]
        public ActionResult Edit(ClubModel club)
        {
            club.Update();
            ViewData.ModelState.Clear();
            ViewBag.Feedback = "Klubb lagret!";
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create", new ClubModel());
        }

        [HttpPost]
        public ActionResult Create(ClubModel model)
        {
            var club = new ClubModel(model.Name);
            try
            {
                club.SaveToDb();
                ViewData.ModelState.Clear();
                ViewBag.Feedback = "Klubb lagret";
            }
            catch (Exception)
            {
                ViewBag.Feedback = "Det skjedde en feil under lagring";
            }
            return View("Create");
        }
    }
}
