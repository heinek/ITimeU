﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class RaceController : Controller
    {
        //
        // GET: /Race/

        public ActionResult Index()
        {
            ViewBag.Error = false;
            return View();            
        }
        
        public ActionResult Create(Race newRace)      //string name, string distance, string startDate)
        {
            RaceModel race = new RaceModel();
            if (IsValidInput(newRace))
            {
                ViewBag.Error = false;
                //race.InsertRace(newRace);
            }
            else
            {
                ViewBag.Error = true;
                ViewBag.Message = "All fields must not be empty";
            }
            return View("Index");

                //int intDistance;            
                //int.TryParse(distance, out intDistance);
                //Race newRace = new Race();
                //newRace.Name = name;
                //newRace.Distance = intDistance;
                //newRace.StartDate = Convert.ToDateTime(startDate);                
                       
        }

        private bool IsValidInput(Race checkRace)
        {
            var check = true;

            if (String.IsNullOrEmpty(checkRace.Name))
                check = false;
            return check;
        }
    }
}
