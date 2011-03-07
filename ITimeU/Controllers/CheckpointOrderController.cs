using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class CheckpointOrderController : Controller
    {
        //
        // GET: /CheckpointOrder/

        public ActionResult Index()
        {
            var checkpoint = new CheckpointOrderModel();
            Session["checkpoint"] = checkpoint;

            var entities = new Entities();
            ViewBag.Checkpoints = entities.Checkpoints.ToList();
            

            return View(checkpoint);
        }

        //
        // GET: /CheckpointOrder/Details/5

        public ActionResult Details(int id)
        {
            return View(GetCheckpointOrder(id));
        }

        //
        // GET: /CheckpointOrder/Create

        [HttpGet]
        public ActionResult Create()
        {
            var entities = new Entities();
            ViewBag.Checkpoints = entities.Checkpoints.ToList();

            //CheckpointOrderModel checkpointOrderModel = new CheckpointOrderModel();
            //checkpointOrderModel.OrderNumber = (int)(entities.CheckpointOrders.OrderByDescending(m => m.OrderNumber).First()).OrderNumber + 1;

            
            //return View(checkpointOrderModel);
            return View("Create", new CheckpointOrderModel());
        } 

        //
        // POST: /CheckpointOrder/Create

        [HttpPost]
        public ActionResult Create(CheckpointOrder CheckpointOrderToInsert)
        {
           return View("Index");
        }

        //
        // GET: /CheckpointOrder/Edit/5
 
        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
            //return View(GetCheckpointOrder(id));
        }

        //
        // POST: /CheckpointOrder/Edit/5

        [HttpPost]
        public ActionResult Edit(int ID, string value)
        {
            using (var ctx = new Entities())
            {
                //CheckpointOrderModel model = new CheckpointOrderModel();
                CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];

                int tmpValue = 0;
                if (!int.TryParse(value, out tmpValue))
                {
                    return null;
                }
                tmpValue = int.Parse(value);
                CheckpointOrder origCheckpointOrder = CheckpointOrderModel.GetCheckpointOrderById(ID);

                //return View();
                return View("Index", model);
            }
        }

        //
        // GET: /CheckpointOrder/Delete/5
 
        public ActionResult DeleteCheckpointOrder(int id)
        {
            try
            {
                //CheckpointOrderModel model = new CheckpointOrderModel();
                CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
                model.DeleteCheckpointOrderDB(id);
                return Content(model.CheckpointOrderDic.ToListboxvalues(false));
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /CheckpointOrder/Delete/5

        //[HttpPost]
        //public ActionResult DeleteCheckpointOrder(string id)
        //{
        //    try
        //    {
        //        CheckpointOrderModel model = new CheckpointOrderModel();
        //        //CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
        //        int idToDelete;
        //        int.TryParse(id, out idToDelete);
        //        model.DeleteCheckpointOrderDB(idToDelete);
        //        return Content(model.CheckpointOrderDic.ToListboxvalues(true));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        private CheckpointOrder GetCheckpointOrder(int id)
        {
            Entities ent = new Entities();
            var coQuery = from c in ent.CheckpointOrders
                          where c.CheckpointID == id
                          select c;
            CheckpointOrder co = coQuery.FirstOrDefault();
            return co;
        }

        public ActionResult AddCheckpointOrder(string checkpointID, string startingNumber)
        {
            //CheckpointOrderModel model = new CheckpointOrderModel();
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            int chkpntID;
            int startNmb;

            int.TryParse(checkpointID, out chkpntID);
            int.TryParse(startingNumber, out startNmb);

            model.AddCheckpointOrderDB(chkpntID, startNmb);
            //Session["checkpoint"] = model; //TODO change this session if does not work
            
            return Content(model.CheckpointOrderDic.ToListboxvalues(false));
        }
       

        public ActionResult UpdateCheckpointOrder(int ID, string startingNumber)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            int StartNmb;

            int.TryParse(startingNumber, out StartNmb);

            model.UpdateCheckpointOrderDB(ID, StartNmb);
            //Session["checkpoint"] = model;
            return Content(model.CheckpointOrderDic.ToListboxvalues(false));
        }

        public ActionResult GetStartingNumbersForCheckpoint(int checkpointID)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.GetStartingNumbersForCheckpoint(checkpointID);
            //Session["checkpoint"] = model;
            return Content(model.CheckpointOrderDic.ToListboxvalues(false));
        }

    }
}
