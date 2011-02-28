using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITimeU.Controllers
{
    public class MockController : Controller
    {

        public ViewResult TestSession()
        {
            Session["item2"] = "This is used for testing a mock session.";
            return View();
        }

        //
        // GET: /Mock/

        public ActionResult Index()
        {
            throw new InvalidOperationException("This controller exists only for testing purposes.");
        }


    }
}
