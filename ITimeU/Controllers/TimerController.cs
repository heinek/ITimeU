using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class TimerController : Controller
    {
        //
        // GET: /Timer/

        public ActionResult Index()
        {
            
            return View("Index");
        }
     
        public ActionResult StartTimer()
        {
            TimerModel timerModel = new TimerModel();
            timerModel.Start();

            return View(timerModel);
        }

    }
}
