using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class ParticipantController : Controller
    {
        //
        // GET: /Participant/

        [HttpGet]
        public ActionResult Index()
        {
            var participantModel = new AthleteModel("Test", "Person");
            return View("Index", participantModel);
        }
    }
}
