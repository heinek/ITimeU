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

        public int AddCheckpointOrderDB(int checkpointId, int startingNumber)
        {
            var entities = new Entities();

            if (CheckpointWithStartNumberExists(checkpointId, startingNumber, entities))
                return 0;
            else
                return CreateCheckpointWithStartNumber(checkpointId, startingNumber, entities);
        }

        private static bool CheckpointWithStartNumberExists(int checkpointId, int startingNumber, Entities entities)
        {
            return entities.CheckpointOrders.Any(
                chkpnt =>
                    (chkpnt.StartingNumber == startingNumber &&
                    chkpnt.CheckpointID == checkpointId));
        }

        private int CreateCheckpointWithStartNumber(int checkpointId, int startingNumber, Entities entities)
        {
            var checkpointOrder = CreateCheckpointOrder(checkpointId, startingNumber, entities);
            SaveCheckpointOrder(checkpointOrder, entities);
            AddToCheckpointOrderDic(checkpointId, entities);

            return checkpointOrder.ID;
        }

        private static CheckpointOrder CreateCheckpointOrder(int checkpointId, int startingNumber, Entities entities)
        {
            var checkpointOrder = new CheckpointOrder();
            checkpointOrder.CheckpointID = checkpointId;
            checkpointOrder.StartingNumber = startingNumber;

            if ((ExistsAnyCheckpointInDb(entities)) && (CheckpointExistsInDb(checkpointId, entities)))
                checkpointOrder.OrderNumber = GetHighestExistingOrderNumberForCheckpoint(checkpointId, entities) + 1;
            else
                checkpointOrder.OrderNumber = 1;

            checkpointOrder.IsDeleted = false;
            return checkpointOrder;
        }

        private static bool ExistsAnyCheckpointInDb(Entities entities)
        {
            return entities.CheckpointOrders.Count() > 0;
        }

        private static bool CheckpointExistsInDb(int checkpointId, Entities entities)
        {
            return entities.CheckpointOrders.Any(chkpnt => chkpnt.CheckpointID == checkpointId);
        }

        private static int? GetHighestExistingOrderNumberForCheckpoint(int checkpointId, Entities entities)
        {
            return entities.CheckpointOrders.Where(
                chkpntid => chkpntid.CheckpointID == checkpointId).
                OrderByDescending(chkpnt => chkpnt.OrderNumber).
                First().OrderNumber;
        }

        private static void SaveCheckpointOrder(CheckpointOrder checkpointOrder, Entities entities)
        {
            entities.CheckpointOrders.AddObject(checkpointOrder);
            entities.SaveChanges();
        }

        private void AddToCheckpointOrderDic(int checkpointId, Entities entities)
        {
            CheckpointOrderDic.Clear();
            IOrderedQueryable<CheckpointOrder> checkpointOrders =
                entities.CheckpointOrders.
                Where(chkpnt => chkpnt.CheckpointID == checkpointId).
                OrderByDescending(ordernum => ordernum.OrderNumber);

            foreach (CheckpointOrder chkpntOrder in checkpointOrders)
                CheckpointOrderDic.Add((int)chkpntOrder.ID, (int)chkpntOrder.StartingNumber);
        }

        public void MoveCheckpointUp(int checkpointId, int startingNumber, int Id)
        {
            using (var ctx = new Entities())
            {
                int orderNumber = 0;
                int nextId = 0;
                int nextOrderNumber = 0;

                if (ctx.CheckpointOrders.Count() > 0)
                {
                    int maxOrderNumber = (int)(ctx.CheckpointOrders.Where(chkpntid => chkpntid.CheckpointID == checkpointId).OrderByDescending(chkpnt => chkpnt.OrderNumber).First().OrderNumber);

                    orderNumber = (int)(ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == Id).OrderNumber);
                    if (orderNumber < maxOrderNumber)
                    {
                        nextOrderNumber = (int)(ctx.CheckpointOrders.Where(chkpnt => (chkpnt.CheckpointID == checkpointId && chkpnt.OrderNumber > orderNumber)).Min(chkpnt => chkpnt.OrderNumber));
                        nextId = (int)(ctx.CheckpointOrders.Single(chkpntid => (chkpntid.OrderNumber == nextOrderNumber && chkpntid.CheckpointID == checkpointId)).ID);
                        using (var editCtx = new Entities())
                        {
                            var insertRecord = editCtx.CheckpointOrders.Single(chkpnt => (chkpnt.ID == Id));
                            insertRecord.OrderNumber = nextOrderNumber;
                            editCtx.SaveChanges();

                            var insertRecord2 = editCtx.CheckpointOrders.Single(chkpnt => (chkpnt.ID == nextId));
                            insertRecord2.OrderNumber = orderNumber;
                            editCtx.SaveChanges();
                        }
                    }
                }

                AddToCheckpointOrderDic(checkpointId, ctx);
            }
        }

        /// <summary>
        /// Moves the checkpoint down.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <param name="startingNumber">The starting number.</param>
        /// <param name="Id">The id.</param>
        public void MoveCheckpointDown(int checkpointId, int startingNumber, int Id)
        {
            using (var ctx = new Entities())
            {

                int orderNumber = 0;
                int previousId = 0;
                int previousOrderNumber = 0;

                if (ctx.CheckpointOrders.Count() > 0)
                {
                    orderNumber = (int)(ctx.CheckpointOrders.Single(chkpnt => chkpnt.ID == Id).OrderNumber);
                    int minOrderNumber = (int)(ctx.CheckpointOrders.Where(chkpntid => chkpntid.CheckpointID == checkpointId).OrderBy(chkpnt => chkpnt.OrderNumber).First().OrderNumber);
                    if (orderNumber > minOrderNumber)
                    {
                        previousOrderNumber = (int)(ctx.CheckpointOrders.Where(chkpnt => (chkpnt.CheckpointID == checkpointId && chkpnt.OrderNumber < orderNumber)).Max(chkpnt => chkpnt.OrderNumber));
                        previousId = (int)(ctx.CheckpointOrders.Single(chkpntid => (chkpntid.OrderNumber == previousOrderNumber && chkpntid.CheckpointID == checkpointId)).ID);
                        using (var editCtx = new Entities())
                        {
                            var insertRecord = editCtx.CheckpointOrders.Single(chkpnt => (chkpnt.ID == Id));
                            insertRecord.OrderNumber = previousOrderNumber;
                            editCtx.SaveChanges();

                            var insertRecord2 = editCtx.CheckpointOrders.Single(chkpnt => (chkpnt.ID == previousId));
                            insertRecord2.OrderNumber = orderNumber;
                            editCtx.SaveChanges();
                        }
                    }
                    else
                    {
                        // TODO alert for not moving Down
                    }
                    //return checkpointOrderModel;
                }

                AddToCheckpointOrderDic(checkpointId, ctx);
            }
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
                return ctx.CheckpointOrders.Where(chkpnt => chkpnt.ID == id).SingleOrDefault();
            }
        }

        public static CheckpointOrderModel GetById(int id)
        {
            using (var ctx = new Entities())
            {
                var checkpointorder = ctx.CheckpointOrders.Where(chkpnt => chkpnt.ID == id).SingleOrDefault();
                var checkpointordermodel = new CheckpointOrderModel()
                {
                    CheckpointID = checkpointorder.CheckpointID.HasValue ? checkpointorder.CheckpointID.Value : 0,
                    ID = checkpointorder.ID,
                    OrderNumber = checkpointorder.OrderNumber.HasValue ? checkpointorder.OrderNumber.Value : 0,
                    StartingNumber = checkpointorder.StartingNumber.HasValue ? checkpointorder.StartingNumber.Value : 0,
                    CheckpointOrderDic = new Dictionary<int, int>()
                };
                return checkpointordermodel;
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

        /// <summary>
        /// Edits the checkpoint order.
        /// </summary>
        /// <param name="cporderid">The cporderid.</param>
        /// <param name="newstartnumber">The newstartnumber.</param>
        public static void EditCheckpointOrder(int cporderid, int newstartnumber)
        {
            using (var context = new Entities())
            {
                var checkpointOrder = context.CheckpointOrders.Where(order => order.ID == cporderid).SingleOrDefault();
                checkpointOrder.StartingNumber = newstartnumber;
                context.SaveChanges();
            }
        }

        public void DeleteAllCheckpointOrdersOnCheckpoint(int checkpointId)
        {
            using (var context = new Entities())
            {
                foreach (CheckpointOrder co in context.CheckpointOrders.Where(chkpnt => chkpnt.CheckpointID == checkpointId))
                {
                    co.IsDeleted = true;
                }
                context.SaveChanges();
            }
        }

    }
}