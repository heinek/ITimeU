using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class ClassController : Controller
    {
        //
        // GET: /Class/

        public ActionResult Index()
        {
            return View(AthleteClassModel.GetAll());
        }

        //
        // GET: /Class/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Class/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Class/Create

        [HttpPost]
        public ActionResult Create(AthleteClassModel model)
        {
            try
            {
                var athleteclass = new AthleteClassModel()
                {
                    Name = model.Name
                };
                athleteclass.SaveToDb();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Class/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Class/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, AthleteClassModel model)
        {
            try
            {
                var athleteclass = AthleteClassModel.GetById(id);
                athleteclass.Name = model.Name;
                athleteclass.Update();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Class/Delete/5

        public ActionResult Delete(int id)
        {
            var athleteclass = AthleteClassModel.GetById(id);
            athleteclass.Delete();
            return RedirectToAction("Index");
        }

        //
        // POST: /Class/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
