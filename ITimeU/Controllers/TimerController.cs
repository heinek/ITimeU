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

        public ActionResult Index(TimerModel timerModel)
        {
            if (timerModel == null)
            {
                timerModel = new TimerModel();
            }
            
            return View("Index",timerModel);
        }
     
        public ActionResult StartTimer(TimerModel timerModel)
        {
            timerModel.Start();
            return View("Index", timerModel);
        }

    }
}
