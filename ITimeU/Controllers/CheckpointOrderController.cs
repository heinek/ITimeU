﻿using System;
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
            return View(entities.CheckpointOrders.ToList());
        }

        //
        // GET: /CheckpointOrder/Details/5

        public ActionResult Details(int id)
        {
            return View("Details");
        }

        //
        // GET: /CheckpointOrder/Create

        public ActionResult Create()
        {
            var entities = new Entities();
            ViewBag.Checkpoints = entities.Checkpoints.ToList();

            return View("Create");
        } 

        //
        // POST: /CheckpointOrder/Create

        [HttpPost]
        public ActionResult Create(CheckpointOrder co, int checkpointId, int startingNumber, int orderNumber)
        {
            if (!ModelState.IsValid)
            {
                // insert into db
                return View("Create", co);
            }

            CheckpointOrderModel.Create(checkpointId, startingNumber, orderNumber);

            return RedirectToAction("Index");
        }

        
        //
        // GET: /CheckpointOrder/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /CheckpointOrder/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
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

    }
}
