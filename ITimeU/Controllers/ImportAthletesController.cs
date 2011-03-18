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
        public const string ERROR_NO_FILE_UPLOADED = "Feil: Ingen fil valgt. Velg en fil før du laster opp.";
        public const string TEMPDATA_KEY_UPLOADED_FILE = "uploadedDbFile";

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ImportFromFriRes()
        {
            try
            {
                string accessDbFile = UploadDbFileFromPost();
                TempData[TEMPDATA_KEY_UPLOADED_FILE] = accessDbFile;
                List<AthleteModel> athletes = TryToImportFrom(accessDbFile);
                ViewBag.Athletes = athletes;

                return View("Index", athletes);

            }
            catch (HttpException e)
            {
                return CreateErrorView(e);
            }
        }

        private List<AthleteModel> TryToImportFrom(string accessDbFile)
        {
            List<AthleteModel> athletes = ImportAthletesFrom(accessDbFile);
            return athletes;
        }

        private string UploadDbFileFromPost()
        {
            HttpPostedFileBase file = ValidateUpload();

            string filename = SaveFile(file);
            return filename;
        }

        private HttpPostedFileBase ValidateUpload()
        {
            if (Request.Files.Count == 0)
                throw new HttpException(ERROR_NO_FILE_UPLOADED);

            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength < 0)
                throw new HttpException(ERROR_NO_FILE_UPLOADED);

            return file;
        }

        private string GetFullFileNameFromRequestFile(HttpPostedFileBase file) {
            return Server.MapPath("~" + "/upload/" + System.IO.Path.GetFileName(file.FileName));;
        }

        private string SaveFile(HttpPostedFileBase file)
        {
            string filename = GetFullFileNameFromRequestFile(file);
            file.SaveAs(filename);
            return filename;
        }

        private List<AthleteModel> ImportAthletesFrom(string accessDbFile)
        {
            FriResImporter importer = new FriResImporter(accessDbFile);
            List<AthleteModel> athletes = importer.getAthletes();
            return athletes;
        }

        private ActionResult CreateErrorView(HttpException e)
        {
            ViewBag.Error = e.Message;
            return View("Index");
        }

        public ActionResult ImportUploadedAthletes()
        {
            string accessDbFile = (String) TempData[TEMPDATA_KEY_UPLOADED_FILE];
            List<AthleteModel> athletes = TryToImportFrom(accessDbFile);
            AthleteModel.SaveToDb(athletes);

            return RedirectToAction("ImportCompleted");
        }

        public ActionResult ImportCompleted()
        {
            return View("ImportCompleted");
        }

    }
}
