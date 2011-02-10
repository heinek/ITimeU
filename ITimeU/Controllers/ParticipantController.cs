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
            var participant = new ParticipantModel();

            if (Session["participant"] == null)
            {
                Session["participant"] = participant;
            }
            else
            {
                participant = (ParticipantModel)Session["participant"];
            }
            return View("Index", participant);
        }
    }
}
