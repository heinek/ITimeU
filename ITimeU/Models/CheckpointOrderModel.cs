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

        public void AddCheckpointOrderDB(int checkpointId, int startingNumber)
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

                var dic = CheckpointOrderDic.OrderByDescending(kvp => kvp.Key);

                CheckpointOrderDic = new Dictionary<int, int>(dic.ToDictionary(x => x.Key, y => y.Value));


                //foreach (KeyValuePair<int, int> dic in copy.OrderByDescending(dic => dic.Key))
                //{
                //    CheckpointOrderDic.Key = dic.Key;
                //}

            }
            //return checkpointOrderModel;
        }

        public void AddCheckpointByOrderDB(int checkpointId, int startingNumber, int orderNumber)
        {
            //CheckpointOrderModel checkpointOrderModel; = new CheckpointOrderModel();
            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder;
                int delOrderNumber = 0;
                //CheckpointOrder deleteMovedRecord;
                //checkpointOrder.StartingNumber = startingNumber;
                int maxOrderNum = 0;

                if (ctx.CheckpointOrders.Count() > 0)
                {
                    //checkpointOrder.OrderNumber = (ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.OrderNumber).First().OrderNumber) + 1;
                    maxOrderNum = (int)(ctx.CheckpointOrders.OrderByDescending(ordernum => ordernum.OrderNumber).First().OrderNumber);
                }
                else
                {
                    // checkpointOrder.OrderNumber = 1;                    
                }

                if (orderNumber <= maxOrderNum)
                {
                    foreach (CheckpointOrder checkpointDB in ctx.CheckpointOrders.Where(chkpntid => chkpntid.CheckpointID == checkpointId).OrderByDescending(ordernum => ordernum.OrderNumber))
                    {
                        if (checkpointDB.OrderNumber >= orderNumber)
                        {
                            using (var context = new Entities())
                            {
                                checkpointOrder = new CheckpointOrder();
                                checkpointOrder.CheckpointID = checkpointId;
                                checkpointOrder.StartingNumber = checkpointDB.StartingNumber;
                                delOrderNumber = (int)(checkpointDB.OrderNumber);
                                checkpointOrder.OrderNumber = checkpointDB.OrderNumber + 1;
                                checkpointOrder.IsDeleted = false;
                                context.CheckpointOrders.AddObject(checkpointOrder);
                                context.SaveChanges();
                                if (delOrderNumber != orderNumber)
                                {
                                    var deleteMovedRecord = context.CheckpointOrders.Single(chkpnt => (chkpnt.CheckpointID == checkpointId && (chkpnt.OrderNumber) == delOrderNumber));
                                    context.DeleteObject(deleteMovedRecord);
                                    context.SaveChanges();
                                }
                            }

                        }
                    }
                    using (var context2 = new Entities())
                    {
                        //checkpointOrder = new CheckpointOrder();
                        var insertRecord = context2.CheckpointOrders.Single(chkpnt => (chkpnt.CheckpointID == checkpointId && (chkpnt.OrderNumber) == orderNumber));
                        insertRecord.StartingNumber = startingNumber;
                        //context2.CheckpointOrders.(insertRecord);
                        context2.SaveChanges();
                    }
                }

                //checkpointOrder.IsDeleted = false;
                //ctx.CheckpointOrders.AddObject(checkpointOrder);
                //ctx.SaveChanges();
                // TODO Update Dictionary here
                //checkpointOrderModel.ID = (int)ctx.CheckpointOrders.OrderByDescending(chkpnt => chkpnt.ID).First().ID;
                //CheckpointOrderDic.Add(checkpointOrderModel.ID, startingNumber);

                //var dic = CheckpointOrderDic.OrderByDescending(kvp => kvp.Key);

                //CheckpointOrderDic = new Dictionary<int, int>(dic.ToDictionary(x => x.Key, y => y.Value));                

            }
        }
        public void UpdateCheckpointOrderDB(int ID, int StartingNumber)
        {
            using (var ctx = new Entities())
            {
                CheckpointOrder checkpointOrder = ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == ID);
                checkpointOrder.StartingNumber = StartingNumber;
                //ctx.CheckpointOrders.AddObject(checkpointOrder);
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
                return ctx.CheckpointOrders.Where(checkpointorder => checkpointorder.CheckpointID == checkpointId && !checkpointorder.IsDeleted && !checkpointorder.IsMerged).OrderBy(checkpoint => checkpoint.OrderNumber).ToList();
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

        public void GetStartingNumbersForCheckpoint(int checkpointID)
        {

            using (var ctx = new Entities())
            {
                CheckpointOrderDic.Clear();
                foreach (CheckpointOrder chkpntOrder in ctx.CheckpointOrders.Where(chkpnt => chkpnt.CheckpointID == checkpointID && !chkpnt.IsDeleted && !chkpnt.IsMerged).OrderByDescending(ordernum => ordernum.OrderNumber))
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

        /// <summary>
        /// Deletes the checkpoint order.
        /// </summary>
        /// <param name="checkpointOrderId">The checkpoint order id.</param>
        public static void DeleteCheckpointOrder(int checkpointOrderId)
        {
            using (var context = new Entities())
            {
                var checkpointOrder = context.CheckpointOrders.Where(order => order.ID == checkpointOrderId).SingleOrDefault();
                checkpointOrder.IsDeleted = true;
                context.SaveChanges();
            }
        }
    }
}