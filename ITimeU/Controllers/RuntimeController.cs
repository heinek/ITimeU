using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITimeU.Logging;
using ITimeU.Tests.Models;

namespace ITimeU.Controllers
{
    public class RuntimeController : Controller
    {
     
        public ActionResult Save(string runtime)
        {
            RuntimeModel runtimeDb = RuntimeModel.Create(Int16.Parse(runtime));
            string reply = runtimeDb.Id.ToString();

            // Show ID of created object (and only that).
            return Content(reply);
        }

    }
}
