using System.Linq;
using System.Web.Mvc;
using ITimeU.Models;

namespace ITimeU.Controllers
{
    public class CheckpointOrderController : Controller
    {
        //
        // GET: /CheckpointOrder/

        public ActionResult Index(int raceId)
        {
            var checkpoint = new CheckpointOrderModel();
            Session["checkpoint"] = checkpoint;
            ViewBag.RaceId = raceId;
            var race = RaceModel.GetById(raceId);
            ViewBag.RaceName = race.Name;
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(raceId);
            return View(checkpoint);
        }

        //
        // GET: /CheckpointOrder/Details/5

        public ActionResult Details(int id)
        {
            return View(GetCheckpointOrder(id));
        }

        //
        // GET: /CheckpointOrder/Create

        [HttpGet]
        public ActionResult Create(int raceId)
        {
            ViewBag.Checkpoints = CheckpointModel.GetCheckpoints(raceId);
            return View("Create", new CheckpointOrderModel());
        }

        //
        // POST: /CheckpointOrder/Create

        [HttpPost]
        public ActionResult Create(CheckpointOrder CheckpointOrderToInsert)
        {
            return View("Index");
        }

        //
        // GET: /CheckpointOrder/Edit/5

        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
        }

        //
        // POST: /CheckpointOrder/Edit/5

        [HttpPost]
        public ActionResult Edit(int ID, string value)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];

                int tmpValue = 0;
                if (!int.TryParse(value, out tmpValue))
                {
                    return null;
                }
                tmpValue = int.Parse(value);
                CheckpointOrder origCheckpointOrder = CheckpointOrderModel.GetCheckpointOrderById(ID);
                TimeMergerModel.Merge(origCheckpointOrder.CheckpointID.Value);
                return View("Index", model);
            }
        }

        //
        // GET: /CheckpointOrder/Delete/5

        public ActionResult DeleteCheckpointOrder(int id, int checkpointId)
        {
            try
            {
                //CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
                var model = CheckpointOrderModel.GetById(id);
                model.DeleteCheckpointOrderDB();
                return Content(CheckpointOrderModel.GetCheckpointOrders(checkpointId).ToListboxvalues());
            }
            catch
            {
                TimeMergerModel.Merge(checkpointId);
                return View();
            }
        }

        private CheckpointOrder GetCheckpointOrder(int id)
        {
            Entities ent = new Entities();
            var coQuery = from c in ent.CheckpointOrders
                          where c.CheckpointID == id
                          select c;
            CheckpointOrder co = coQuery.FirstOrDefault();
            return co;
        }

        public ActionResult AddCheckpointOrder(int checkpointID, int startingNumber)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.AddCheckpointOrderDB(checkpointID, startingNumber);
            TimeMergerModel.Merge(checkpointID);
            return Content(model.CheckpointOrderDic.ToListboxvalues(toTimer: false));
        }

        public ActionResult MoveCheckpointUp(int checkpointID, int startingNumber, int checkpointOrderId)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.MoveCheckpointUp(checkpointID, startingNumber, checkpointOrderId);
            TimeMergerModel.Merge(checkpointID);
            return Content(model.CheckpointOrderDic.ToListboxvalues());
        }

        public ActionResult MoveCheckpointDown(int checkpointID, int startingNumber, int checkpointOrderId)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.MoveCheckpointDown(checkpointID, startingNumber, checkpointOrderId);
            TimeMergerModel.Merge(checkpointID);
            return Content(model.CheckpointOrderDic.ToListboxvalues(toTimer: false));
        }

        public ActionResult UpdateCheckpointOrder(int ID, int startingNumber)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.UpdateCheckpointOrderDB(ID, startingNumber);
            CheckpointOrder origCheckpointOrder = CheckpointOrderModel.GetCheckpointOrderById(ID);
            TimeMergerModel.Merge(origCheckpointOrder.CheckpointID.Value);
            return Content(model.CheckpointOrderDic.ToListboxvalues(toTimer: false));
        }

        public ActionResult GetStartingNumbersForCheckpoint(int checkpointID)
        {
            CheckpointOrderModel model = (CheckpointOrderModel)Session["checkpoint"];
            model.GetStartingNumbersForCheckpoint(checkpointID);
            return Content(model.CheckpointOrderDic.ToListboxvalues(toTimer: false));
        }

    }
}
