using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ITimeU.Models
{
    [Serializable]
    public class CheckpointOrderModel
    {
        private int _ID;
        private int _checkpointID;
        private int _startingNumber;
        private int _orderNumber;
        public Dictionary<int, int> CheckpointOrderDic { get; set; }

        public CheckpointOrderModel()
        {
            _ID = -1;
            _checkpointID = -1;
            _startingNumber = -1;
            _orderNumber = -1;
            CheckpointOrderDic = new Dictionary<int, int>();
        }

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public int CheckpointID
        {
            get
            {
                return _checkpointID;
            }
            set
            {
                _checkpointID = value;
            }
        }
        public int StartingNumber
        {
            get
            {
                return _startingNumber;
            }
            set
            {
                _startingNumber = value;
            }
        }
        public int OrderNumber
        {
            get
            {
                return _orderNumber;
            }
            set
            {
                _orderNumber = value;
            }
        }

        public CheckpointOrderModel AddCheckpointOrderDB(int checkpointId, int startingNumber)
        {
            CheckpointOrderModel checkpointOrderModel = new CheckpointOrderModel();

            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder = new CheckpointOrder();
                checkpointOrder.CheckpointID = checkpointId;
                checkpointOrder.StartingNumber = startingNumber;

                if (ctx.CheckpointOrders.Count() > 0)
                {
                    checkpointOrder.OrderNumber = (ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.OrderNumber).First().OrderNumber) + 1;
                }
                else
                {
                    checkpointOrder.OrderNumber = 1;
                }

                checkpointOrder.IsDeleted = false;
                ctx.CheckpointOrders.AddObject(checkpointOrder);
                ctx.SaveChanges();
                checkpointOrderModel.ID = (int)ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.ID).First().ID;

                CheckpointOrderDic.Add(checkpointOrderModel.ID, startingNumber);
            }
            return checkpointOrderModel;
        }

        public void UpdateCheckpointOrderDB(int ID, int StartingNumber)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder = ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == ID);
                checkpointOrder.StartingNumber = StartingNumber;
                ctx.SaveChanges();

                CheckpointOrderDic.Remove(ID);
                CheckpointOrderDic.Add(ID, StartingNumber);
            }
        }

        public void DeleteCheckpointOrderDB(int checkpointOrderId)
        {
            CheckpointOrderDic.Remove(checkpointOrderId);

            using (var ctx = new Entities())
            {
                var checkpointOrderToDelete = ctx.CheckpointOrders.Where(checkpointOrder => checkpointOrder.ID == checkpointOrderId).Single();
                ctx.CheckpointOrders.DeleteObject(checkpointOrderToDelete);
                ctx.SaveChanges();
            }
        }

        public static CheckpointOrder GetCheckpointOrderById(int id)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointToReturn = (CheckpointOrder)ctx.CheckpointOrders.Where(chkpnt => chkpnt.ID == id);
                return checkpointToReturn;
            }
        }

        public static List<CheckpointOrder> GetCheckpointOrders(int checkpointId)
        {
            using (var ctx = new Entities())
            {
                return ctx.CheckpointOrders.Where(checkpointorder => checkpointorder.CheckpointID == checkpointId && checkpointorder.IsDeleted == false).OrderBy(checkpoint => checkpoint.OrderNumber).ToList();
            }
        }

        public static List<CheckpointOrder> GetAllCheckpointOrders()
        {
            using (var ctx = new Entities())
            {
                IEnumerable<Checkpoint> checkpoints = ctx.Checkpoints.AsEnumerable<Checkpoint>();

                List<CheckpointOrder> checkpointOrders = new List<CheckpointOrder>();
                foreach (CheckpointOrder checkpointOrder in checkpointOrders)
                {

                }

                return checkpointOrders;
            }
        }

        public void GetStartingNumbersForCheckpoint(int checkpointID)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrderDic.Clear();
                foreach (CheckpointOrder chkpntOrder in ctx.CheckpointOrders.Where(chkpnt => chkpnt.CheckpointID == checkpointID))
                {
                    CheckpointOrderDic.Add((int)chkpntOrder.ID, (int)chkpntOrder.StartingNumber);
                }
            }
        }

        public IEnumerable<SelectListItem> Checkpoints()
        {
            //return new SelectList(new List<string>() { "1", "2" });
            using (var ctx = new Entities())
            {
                return new SelectList(ctx.Checkpoints.ToList(), "CheckpointID", "Name");
            }
        }

        public IEnumerable<SelectListItem> CheckpointOrders()
        {
            using (var ctx = new Entities())
            {
                return new SelectList(ctx.CheckpointOrders.ToList(), "ID", "OrderNumber");
            }
        }


    }
}