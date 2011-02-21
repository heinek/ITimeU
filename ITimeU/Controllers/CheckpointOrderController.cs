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
            var entities = new Entities();
            ViewBag.Checkpoints = entities.Checkpoints.ToList();
            return View(new CheckpointOrderModel());
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
            return View(GetCheckpointOrder(id));
        }

        //
        // POST: /CheckpointOrder/Edit/5

        [HttpPost]
        public ActionResult Edit(CheckpointOrder co)
        {
            try
            {
                Entities ent = new Entities();
                CheckpointOrder origCheckpointOrder = GetCheckpointOrder((int)co.CheckpointID);
                ent.CheckpointOrders.Attach(co);
                ent.ApplyOriginalValues("CheckpointOrders", origCheckpointOrder);
                ent.SaveChanges();
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /CheckpointOrder/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CheckpointOrder/Delete/5

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

        [NonAction]
        private CheckpointOrder GetCheckpointOrder(int id)
        {
            Entities ent = new Entities();
            var coQuery = from c in ent.CheckpointOrders
                          where c.CheckpointID == id
                          select c;
            CheckpointOrder co = coQuery.FirstOrDefault();
            return co;
        }

        public ActionResult AddStartingNumber(string checkpointID, string startingNumber)
        {
            using (var entities = new Entities())
            {
                CheckpointOrder CheckpointOrderToInsert = new CheckpointOrder();

                int nextOrderNumber = 1;
                if (entities.CheckpointOrders.Count() > 0)
                {
                    nextOrderNumber = (int)(entities.CheckpointOrders.OrderByDescending(m => m.OrderNumber).First()).OrderNumber + 1;
                }

                CheckpointOrderToInsert.OrderNumber = nextOrderNumber;
                CheckpointOrderToInsert.CheckpointID = int.Parse(checkpointID);
                CheckpointOrderToInsert.StartingNumber = int.Parse(startingNumber);

                entities.AddToCheckpointOrders(CheckpointOrderToInsert);
                entities.SaveChanges();

                return null;
            }
        }

    }
}
