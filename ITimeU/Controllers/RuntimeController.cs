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
     
        /// <summary>
        /// Saves the runtime to database.
        /// </summary>
        /// <param name="runtime">The runtime in milliseconds. Maximum value is 2 147 483 647
        /// (Int32.MaxValue), which equals 24,86 days.</param>
        /// <returns>The primary key ID of the database row insterted.</returns>
        public ActionResult Save(string runtime)
        {
            RuntimeModel runtimeDb = RuntimeModel.Create(Int32.Parse(runtime));
            string reply = runtimeDb.Id.ToString();

            // Show ID of created object (and only that).
            return Content(reply);
        }

    }
}
