using System;
//using System.Linq;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class CheckpointController : Controller
    {
        public const string KEY_A_CHECKPOINT_WAS_CREATED_SUCCESSFULLY = "CheckpointCreatedSuccessfully";
        public const string KEY_NAME_EMPTY = "CheckpointNameEmpty";
        //
        // GET: /Checkpoint/

        public ActionResult Index(int? id)
        {
            if (id != null)
                return RedirectToAction("Index", "Timer", new { checkpoint_id = id });

            return View(CheckpointModel.getAll());
        }

        public ActionResult Create()
        {
            if (CheckpointWasCreatedSuccessfully())
                ViewBag.CheckpointCreatedSuccessfully = true;
            else
                ViewBag.CheckpointCreatedSuccessfully = false;

            if (IsCheckpointNameEmpty())
                ViewBag.IsCheckpointEmpty = true;
            else
                ViewBag.IsCheckpointEmpty = false;

            return View("Create", RaceModel.GetRaces());
        }

        private bool CheckpointWasCreatedSuccessfully()
        {
            return TempData.Keys.Contains(KEY_A_CHECKPOINT_WAS_CREATED_SUCCESSFULLY) &&
                (bool)GetTempData(KEY_A_CHECKPOINT_WAS_CREATED_SUCCESSFULLY) == true;
        }

        private bool IsCheckpointNameEmpty()
        {
            return TempData.Keys.Contains(KEY_NAME_EMPTY) &&
                (bool)GetTempData(KEY_NAME_EMPTY) == true;
        }
        [HttpPost]
        public ActionResult Create(int raceId, string txtCheckpointName)
        {
            if (String.IsNullOrEmpty(txtCheckpointName))
            {
                SetTempData(KEY_NAME_EMPTY, true);
            }
            else
            {
                CheckpointModel model = new CheckpointModel(txtCheckpointName, raceId); // race with raceId exists in the database already.
                model.SaveToDb();
                SetTempData(KEY_A_CHECKPOINT_WAS_CREATED_SUCCESSFULLY, true);
            }
            return RedirectToAction("Create"); // Redirect in order to reset form values.
        }


        private void SetTempData(string key, object value)
        {
            TempData[key] = value;
        }

        private object GetTempData(string key)
        {
            return TempData[key]; ;
        }

        public ActionResult List()
        {
            return View("List", CheckpointModel.getAll());
        }

        public ActionResult Delete(int id)
        {
            var checkpoint = CheckpointModel.getById(id);
            checkpoint.Delete();
            return View("List", CheckpointModel.getAll());
        }


        private void SetTempData(string key, object value)
        {
            TempData[key] = value;
        }

        private object GetTempData(string key)
        {
            return TempData[key]; ;
        }


    }
}

