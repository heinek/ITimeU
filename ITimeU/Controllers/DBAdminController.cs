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
            return View();
        }

        [HttpPost]
        public ActionResult DeleteDB()
        {
            string result = "";
            if (DBAdminModel.DeleteAll())
            {
                result = "All data har blitt slettet";
            }
            else
            {
                result = "Det skjedde en feil under nullstilling av databasen";
            }
            return Content(result);
        }
    }
}
