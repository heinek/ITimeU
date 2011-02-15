using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITimeU.Models
{
    public class CheckpointOrderModel
    {
        private int _checkpointID;
        private int _startingNumber;
        private int _orderNumber;

        public CheckpointOrderModel()
        {
            _checkpointID = 0;
            _startingNumber = 0;
            _orderNumber = 0;
        }

        public int ID { get; set; }
        public int CheckpointID 
        { 
            get
            {
                return _checkpointID;
            }
            set
            {
            }
        }
        public int StartingNumber { get; set; }
        public int OrderNumber { get; set; }

        public static CheckpointOrderModel Create(int checkpointId, int startingNumber, int orderNumber)
        {
            CheckpointOrderModel checkpointOrderModel = new CheckpointOrderModel();

            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder = new CheckpointOrder();
                checkpointOrder.CheckpointID = checkpointId;
                checkpointOrder.StartingNumber = startingNumber;
                checkpointOrder.StartingNumber = (ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.CheckpointID).First().CheckpointID) + 1;
                ctx.CheckpointOrders.AddObject(checkpointOrder);
                ctx.SaveChanges();
                checkpointOrder.ID = (int)ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.CheckpointID).First().CheckpointID;
            }

            return checkpointOrderModel;
        }

        public void Save(int CheckpointID, int StartingNumber, int OrderNumber)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder = ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == ID);
  
                checkpointOrder.CheckpointID = CheckpointID;
                checkpointOrder.StartingNumber = StartingNumber;
                checkpointOrder.OrderNumber = OrderNumber;
                
                ctx.SaveChanges();
            }
        }

        public static CheckpointOrderModel GetCheckpointOrderById(int id)
        {
            using (var ctx = new Entities())
            {
                var checkpointOrder = ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == id);
                var checkpointModel = new CheckpointOrderModel()
                {
                    ID = checkpointOrder.ID,
                    CheckpointID = (int)checkpointOrder.CheckpointID,
                    StartingNumber = (int)checkpointOrder.StartingNumber,
                    OrderNumber = (int)checkpointOrder.OrderNumber
                };
                return checkpointModel;
            }
        }

        public static List<CheckpointOrder> GetCheckpointOrders()
        {
            using (var ctx = new Entities())
            {
                return ctx.CheckpointOrders.Select(chkpnt =>
                    new CheckpointOrder()
                    {
                        CheckpointID = chkpnt.CheckpointID,
                        StartingNumber = chkpnt.StartingNumber,
                        OrderNumber = chkpnt.OrderNumber
                    }).ToList();
            }
        }

    }
}