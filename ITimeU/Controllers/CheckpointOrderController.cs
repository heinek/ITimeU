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
            return View(CheckpointOrderModel.GetCheckpointOrders());
        }

    }
}
