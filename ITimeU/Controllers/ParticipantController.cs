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
            var participantModel = new ParticipantModel();
            return View("Index", participantModel);
        }
    }
}
