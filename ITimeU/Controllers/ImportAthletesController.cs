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
        public const string ERROR_EMPTY_DATABASE = "Feil: Databasen du lastet opp inneholder ingen deltakere.";
        public const string TEMPDATA_KEY_UPLOADED_FILE = "uploadedDbFile";
        public const string TEMPDATA_KEY_NUMBER_OF_IMPORTED_ATHLETES = "numberOfImportedAthletes";

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ImportFromFriRes()
        {
            string accessDbFile = null;

            try
            {
                accessDbFile = UploadDbFileFromPost();

            }
            catch (HttpException e)
            {
                return CreateErrorView(e.Message);
            }

            SetTempData(TEMPDATA_KEY_UPLOADED_FILE, accessDbFile);
            List<AthleteModel> athletes = TryToImportFrom(accessDbFile);

            if (athletes.Count == 0)
                return CreateErrorView(ERROR_EMPTY_DATABASE);

            return View("Index", athletes);
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
            if (file.ContentLength <= 0)
                throw new HttpException(ERROR_NO_FILE_UPLOADED);

            return file;
        }

        private string SaveFile(HttpPostedFileBase file)
        {
            string filename = GetFullFileNameFromRequestFile(file);
            file.SaveAs(filename);
            return filename;
        }

        private string GetFullFileNameFromRequestFile(HttpPostedFileBase file) {
            return Server.MapPath("~" + "/upload/" + System.IO.Path.GetFileName(file.FileName));;
        }

        private ActionResult CreateErrorView(string message)
        {
            ViewBag.Error = message;
            return View("Index");
        }

        private List<AthleteModel> TryToImportFrom(string accessDbFile)
        {
            FriResImporter importer = new FriResImporter(accessDbFile);
            List<AthleteModel> athletes = importer.getAthletes();
            return athletes;
        }

        public ActionResult ImportUploadedAthletes()
        {
            List<AthleteModel> athletes = ImportUploadedAthletesFromTempFile();
            AthleteModel.SaveToDb(athletes);
            SetTempData(TEMPDATA_KEY_NUMBER_OF_IMPORTED_ATHLETES, athletes.Count);

            return RedirectToAction("ImportCompleted");
        }

        private List<AthleteModel> ImportUploadedAthletesFromTempFile()
        {
            string accessDbFile = (String) GetTempData(TEMPDATA_KEY_UPLOADED_FILE);
            List<AthleteModel> athletes = TryToImportFrom(accessDbFile);
            return athletes;
        }

        private object GetTempData(string key)
        {
            return TempData[key]; ;
        }

        public ActionResult ImportCompleted()
        {
            int numberOfImportedAthletes = (int) GetTempData(TEMPDATA_KEY_NUMBER_OF_IMPORTED_ATHLETES);
            // Prevent number of athletes to disappear if user refreshes page.
            SetTempData(TEMPDATA_KEY_NUMBER_OF_IMPORTED_ATHLETES, numberOfImportedAthletes);

            return View("ImportCompleted", numberOfImportedAthletes);
        }

        private void SetTempData(string key, object value)
        {
            TempData[key] = value;
        }

    }
}
