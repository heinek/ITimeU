using System;
using System.Web.Mvc;
using ITimeU.Models;

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
        public ActionResult Save(string runtime, string checkpointId)
        {
            RuntimeModel runtimeDb = RuntimeModel.Create(Int32.Parse(runtime), Int32.Parse(checkpointId));
            string reply = runtimeDb.Id.ToString();

            // Show ID of created object (and only that).
            return Content(reply);
        }

    }
}
