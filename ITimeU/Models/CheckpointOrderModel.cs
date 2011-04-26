using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ITimeU.Models
{
    [Serializable]
    public class CheckpointOrderModel
    {
        public int ID { get; set; }
        public int CheckpointID { get; set; }
        public int StartingNumber { get; set; }
        public int OrderNumber { get; set; }
        public bool IsMerged { get; set; }
        public Dictionary<int, int> CheckpointOrderDic { get; set; }

        public CheckpointOrderModel()
        {
            ID = -1;
            CheckpointID = -1;
            StartingNumber = -1;
            OrderNumber = -1;
            CheckpointOrderDic = new Dictionary<int, int>();
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
            if (startingNumber == 0) return false;
            return entities.CheckpointOrders.Any(
                chkpnt =>
                    (chkpnt.StartingNumber == startingNumber &&
                    chkpnt.CheckpointID == checkpointId && !chkpnt.IsDeleted));
        }

        private int CreateCheckpointWithStartNumber(int checkpointId, int startingNumber, Entities entities)
        {
            var checkpointOrder = CreateCheckpointOrder(checkpointId, startingNumber, entities);
            SaveCheckpointOrder(checkpointOrder, entities);
            AddToCheckpointOrderDic(checkpointId, entities);
            ID = checkpointOrder.ID;
            return checkpointOrder.ID;
        }

        private static CheckpointOrder CreateCheckpointOrder(int checkpointId, int startingNumber, Entities entities)
        {
            var checkpointOrder = new CheckpointOrder();
            checkpointOrder.CheckpointID = checkpointId;
            checkpointOrder.StartingNumber = startingNumber;

            if ((ExistsAnyCheckpointOrdersInDb(entities)) && (CheckpointExistsInDb(checkpointId, entities)))
                checkpointOrder.OrderNumber = GetHighestExistingOrderNumberForCheckpoint(checkpointId, entities) + 1;
            else
                checkpointOrder.OrderNumber = 1;

            checkpointOrder.IsDeleted = false;
            return checkpointOrder;
        }

        private static bool ExistsAnyCheckpointOrdersInDb(Entities entities)
        {
            return entities.CheckpointOrders.Count() > 0;
        }

        private static bool CheckpointExistsInDb(int checkpointId, Entities entities)
        {
            return entities.CheckpointOrders.Any(chkpnt => chkpnt.CheckpointID == checkpointId);
        }

        private static int GetHighestExistingOrderNumberForCheckpoint(int checkpointId, Entities entities)
        {
            return (int)entities.CheckpointOrders.
                Where(chkpntid => chkpntid.CheckpointID == checkpointId).
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
                AddToCheckpointOrdersDictionary((int)chkpntOrder.ID, (int)chkpntOrder.StartingNumber);
        }

        private void AddToCheckpointOrdersDictionary(int checkpointOrderId, int StartingNumber)
        {
            CheckpointOrderDic.Add(checkpointOrderId, StartingNumber);
        }

        public void MoveCheckpointUp(int checkpointId, int startingNumber, int checkpointOrderId)
        {
            var entities = new Entities();

            if (ExistsAnyCheckpointOrdersInDb(entities))
                MoveCheckpointUp(checkpointId, checkpointOrderId, entities);

            AddToCheckpointOrderDic(checkpointId, entities);
        }

        private static void MoveCheckpointUp(int checkpointId, int checkpointOrderId, Entities entities)
        {
            int orderNumber = GetOrderNumber(checkpointOrderId, entities);
            int maxOrderNumber = GetHighestExistingOrderNumberForCheckpoint(checkpointId, entities);

            if (orderNumber < maxOrderNumber)
                SwapCheckpointOrderWithNext(checkpointId, checkpointOrderId, orderNumber, entities);
        }

        private static int GetOrderNumber(int checkpointOrderId, Entities entities)
        {
            return (int)entities.CheckpointOrders.
                Single(checkpointOrder => checkpointOrder.ID == checkpointOrderId).
                OrderNumber;
        }

        private static void SwapCheckpointOrderWithNext(
            int checkpointId, int checkpointOrderId, int orderNumber, Entities entities)
        {
            int nextOrderNumber = GetNextOrderNumber(checkpointId, orderNumber, entities);
            CheckpointOrder originalCheckpointOrder =
                entities.CheckpointOrders.Single(checkpointOrder => (checkpointOrder.ID == checkpointOrderId));
            originalCheckpointOrder.OrderNumber = nextOrderNumber;

            int nextCheckpointOrderId = GetCheckpointOrderId(checkpointId, nextOrderNumber, entities);
            CheckpointOrder nextCheckpointOrder =
                entities.CheckpointOrders.Single(checkpointOrder => (checkpointOrder.ID == nextCheckpointOrderId));
            nextCheckpointOrder.OrderNumber = orderNumber;

            entities.SaveChanges();
        }

        private static int GetNextOrderNumber(int checkpointId, int orderNumber, Entities entities)
        {
            return (int)entities.CheckpointOrders.
                Where(checkpointOrder =>
                    (checkpointOrder.CheckpointID == checkpointId &&
                    checkpointOrder.OrderNumber > orderNumber)).
                Min(checkpointOrder => checkpointOrder.OrderNumber);
        }

        private static int GetCheckpointOrderId(int checkpointId, int orderNumber, Entities entities)
        {
            return entities.CheckpointOrders.
                Single(checkpointOrder =>
                    (checkpointOrder.OrderNumber == orderNumber &&
                        checkpointOrder.CheckpointID == checkpointId)).
                    ID;
        }

        /// <summary>
        /// Moves the checkpoint down.
        /// </summary>
        /// <param name="checkpointId">The checkpoint id.</param>
        /// <param name="startingNumber">The starting number.</param>
        /// <param name="checkpointOrderId">The id.</param>
        public void MoveCheckpointDown(int checkpointId, int startingNumber, int checkpointOrderId)
        {
            var entities = new Entities();

            if (ExistsAnyCheckpointOrdersInDb(entities))
                MoveCheckpointDown(checkpointId, checkpointOrderId, entities);

            AddToCheckpointOrderDic(checkpointId, entities);
        }

        private static void MoveCheckpointDown(int checkpointId, int checkpointOrderId, Entities entities)
        {
            int orderNumber = GetOrderNumber(checkpointOrderId, entities);
            int minOrderNumber = GetLowestExistingOrderNumberForCheckpoint(checkpointId, entities);

            if (orderNumber > minOrderNumber)
                SwapCheckpointOrderWithPrevious(checkpointId, checkpointOrderId, orderNumber, entities);
        }

        private static int GetLowestExistingOrderNumberForCheckpoint(int checkpointId, Entities entities)
        {
            return (int)entities.CheckpointOrders.
                Where(chkpntid => chkpntid.CheckpointID == checkpointId).OrderBy(chkpnt => chkpnt.OrderNumber).
                First().OrderNumber;
        }

        private static void SwapCheckpointOrderWithPrevious(
            int checkpointId, int checkpointOrderId, int orderNumber, Entities entities)
        {
            int previousOrderNumber = GetPreviousOrderNumber(checkpointId, orderNumber, entities);
            CheckpointOrder originalCheckpointOrder =
                entities.CheckpointOrders.Single(chkpnt => (chkpnt.ID == checkpointOrderId));
            originalCheckpointOrder.OrderNumber = previousOrderNumber;

            int previousId = GetCheckpointOrderId(checkpointId, previousOrderNumber, entities);
            CheckpointOrder previousCheckpointOrder =
                entities.CheckpointOrders.Single(chkpnt => (chkpnt.ID == previousId));
            previousCheckpointOrder.OrderNumber = orderNumber;

            entities.SaveChanges();
        }

        private static int GetPreviousOrderNumber(int checkpointId, int orderNumber, Entities entities)
        {
            return (int)entities.CheckpointOrders.
                Where(chkpnt => (chkpnt.CheckpointID == checkpointId && chkpnt.OrderNumber < orderNumber)).
                Max(chkpnt => chkpnt.OrderNumber);
        }

        public void UpdateCheckpointOrderDB(int checkpointOrderId, int StartingNumber)
        {
            UpdateCheckpointOrderInDb(checkpointOrderId, StartingNumber);
            UpdateCheckpointOrderInDictionary(checkpointOrderId, StartingNumber);
        }

        private static void UpdateCheckpointOrderInDb(int checkpointOrderId, int StartingNumber)
        {
            var entities = new Entities();

            CheckpointOrder checkpointOrder = entities.CheckpointOrders.Single(order => order.ID == checkpointOrderId);
            checkpointOrder.StartingNumber = StartingNumber;
            entities.SaveChanges();
        }

        private void UpdateCheckpointOrderInDictionary(int checkpointOrderId, int StartingNumber)
        {
            RemoveFromCheckpointOrdersDictionary(checkpointOrderId);
            AddToCheckpointOrdersDictionary(checkpointOrderId, StartingNumber);
        }

        private void RemoveFromCheckpointOrdersDictionary(int checkpointOrderId)
        {
            CheckpointOrderDic.Remove(checkpointOrderId);
        }

        public void DeleteCheckpointOrderDB()
        {
            RemoveFromCheckpointOrdersDictionary(ID);
            DeleteCheckpointOrderInDb(ID);
        }

        private static void DeleteCheckpointOrderInDb(int checkpointOrderId)
        {
            var entities = new Entities();

            CheckpointOrder checkpointOrderToDelete =
                entities.CheckpointOrders.
                Where(checkpointOrder => checkpointOrder.ID == checkpointOrderId).
                Single();
            entities.CheckpointOrders.DeleteObject(checkpointOrderToDelete);
            entities.SaveChanges();
        }

        public static CheckpointOrder GetCheckpointOrderById(int id)
        {
            return new Entities().CheckpointOrders.Where(chkpnt => chkpnt.ID == id).SingleOrDefault();
        }

        public static CheckpointOrderModel GetById(int id)
        {
            CheckpointOrder checkpointOrder = new Entities().CheckpointOrders.Where(chkpnt => chkpnt.ID == id).SingleOrDefault();
            var checkpointOrderModel = new CheckpointOrderModel()
            {
                CheckpointID = checkpointOrder.CheckpointID.HasValue ? checkpointOrder.CheckpointID.Value : 0,
                ID = checkpointOrder.ID,
                OrderNumber = checkpointOrder.OrderNumber.HasValue ? checkpointOrder.OrderNumber.Value : 0,
                StartingNumber = checkpointOrder.StartingNumber.HasValue ? checkpointOrder.StartingNumber.Value : 0,
                IsMerged = checkpointOrder.IsMerged,
                CheckpointOrderDic = new Dictionary<int, int>()
            };
            return checkpointOrderModel;
        }

        public static List<CheckpointOrder> GetCheckpointOrders(int checkpointId)
        {
            return new Entities().CheckpointOrders.
                Where(checkpointorder =>
                    checkpointorder.CheckpointID == checkpointId &&
                    !checkpointorder.IsDeleted &&
                    !checkpointorder.IsMerged).
                    OrderBy(checkpoint => checkpoint.OrderNumber).
                    ToList();
        }

        public void GetStartingNumbersForCheckpoint(int checkpointID)
        {
            CheckpointOrderDic.Clear();
            IOrderedQueryable<CheckpointOrder> checkpointOrders = new Entities().CheckpointOrders.
                Where(chkpnt =>
                    chkpnt.CheckpointID == checkpointID &&
                    !chkpnt.IsDeleted &&
                    !chkpnt.IsMerged).
                    OrderByDescending(ordernum => ordernum.OrderNumber);

            foreach (CheckpointOrder chkpntOrder in checkpointOrders)
                CheckpointOrderDic.Add((int)chkpntOrder.ID, (int)chkpntOrder.StartingNumber);
        }

        public IEnumerable<SelectListItem> Checkpoints()
        {
            return new SelectList(new Entities().Checkpoints.ToList(), "CheckpointID", "Name");
        }

        public IEnumerable<SelectListItem> CheckpointOrders()
        {
            return new SelectList(new Entities().CheckpointOrders.ToList(), "ID", "OrderNumber");
        }

        /// <summary>
        /// Deletes the checkpoint order.
        /// </summary>
        /// <param name="checkpointOrderId">The checkpoint order id.</param>
        public static void DeleteCheckpointOrder(int checkpointOrderId)
        {
            var entities = new Entities();
            var checkpointOrder =
                entities.CheckpointOrders.Where(order => order.ID == checkpointOrderId).SingleOrDefault();
            checkpointOrder.IsDeleted = true;
            entities.SaveChanges();
        }

        /// <summary>
        /// Edits the checkpoint order.
        /// </summary>
        /// <param name="cporderid">The checkpoint order id.</param>
        /// <param name="newstartnumber">The new startnumber.</param>
        public static void EditCheckpointOrder(int cporderid, int newstartnumber)
        {
            using (var context = new Entities())
            {
                var checkpointOrder =
                    context.CheckpointOrders.Where(order => order.ID == cporderid).SingleOrDefault();
                checkpointOrder.StartingNumber = newstartnumber;
                context.SaveChanges();
            }
        }

        public void DeleteAllCheckpointOrdersOnCheckpoint(int checkpointId)
        {
            var entities = new Entities();
            IQueryable<CheckpointOrder> checkpointOrders =
                entities.CheckpointOrders.Where(chkpnt => chkpnt.CheckpointID == checkpointId);

            foreach (CheckpointOrder co in checkpointOrders)
                co.IsDeleted = true;

            entities.SaveChanges();
        }


        public void Save()
        {
            using (var context = new Entities())
            {
                var cpo = new CheckpointOrder()
                {
                    CheckpointID = CheckpointID,
                    OrderNumber = OrderNumber,
                    StartingNumber = StartingNumber
                };
                context.CheckpointOrders.AddObject(cpo);
                context.SaveChanges();
                ID = cpo.ID;
            }
        }

        public void Update()
        {
            using (var context = new Entities())
            {
                var cpodb = context.CheckpointOrders.Single(cpo => cpo.ID == ID);
                cpodb.IsMerged = IsMerged;
                cpodb.OrderNumber = OrderNumber;
                cpodb.StartingNumber = StartingNumber;
                context.SaveChanges();
            }
        }
    }
}