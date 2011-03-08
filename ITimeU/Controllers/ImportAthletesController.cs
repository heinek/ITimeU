using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;
using ITimeU.Library;

namespace ITimeU.Controllers
{
    public class ImportAthletesController : Controller
    {
        //
        // GET: /ImportAthletes/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromFriRes()
        {
            string accessDbFile = Upload();
            FriResImporter importer = new FriResImporter(accessDbFile);
            List<AthleteModel> athletes = importer.getAthletes();

            ViewBag.Athletes = athletes;

            return View("Index", athletes);
        }


        public string Upload()
        {
            string accessDbFile = null;
            HttpPostedFileBase file = Request.Files["accessDbFile"];
            if (file.ContentLength > 0)
            {
                string filename = Server.MapPath("~" + "/upload/" + System.IO.Path.GetFileName(file.FileName));
                file.SaveAs(filename);
                accessDbFile = filename;
            }

            return accessDbFile;
        }
    }
}
