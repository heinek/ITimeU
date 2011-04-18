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
            if (club.Update())
            {
                ViewData.ModelState.Clear();
                ViewBag.Success = "Klubb lagret!";
            }
            else
                ViewBag.Error = "Det skjedde en feil under oppdatering av klubb.";
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
                ViewBag.Success = "Klubb lagret";
            }
            catch (Exception)
            {
                ViewBag.Error = "Det skjedde en feil under lagring";
            }
            return View("Create");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var club = ClubModel.GetById(id);
            club.DeleteFromDb();
            return View("Index", ClubModel.GetClubs());
        }
    }
}
