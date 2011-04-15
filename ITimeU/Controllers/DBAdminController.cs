using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class DBAdminController : Controller
    {
        //
        // GET: /DBAdmin/

        public ActionResult Index()
        {
            if (Session["success"] != null)
                ViewBag.Success = (string)Session["success"];
            else
                ViewBag.Error = (string)Session["error"];
            return View();
        }

        [HttpPost]
        public ActionResult DeleteDB()
        {
            if (DBAdminModel.DeleteAll())
            {
                Session["success"] = "All data har blitt slettet";
            }
            else
            {
                Session["error"] = "Det skjedde en feil under nullstilling av databasen";
            }
            return View("Index");
        }
    }
}
