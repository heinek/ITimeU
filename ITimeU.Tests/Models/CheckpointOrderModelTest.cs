using System.Collections.Generic;
using System.Linq;
using ITimeU.Logging;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class CheckpointOrderModelTest : ScenarioClass
    {
        private TimeMergerModel timeMerger;
        private TimerModel timer;
        private CheckpointOrderModel checkpointOrderModel;
        private CheckpointModel checkpoint1;
        private CheckpointModel checkpoint2;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestInitialize]
        public void TestSetup()
        {
            timeMerger = new TimeMergerModel();
            timer = new TimerModel();
            checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            checkpointOrderModel = new CheckpointOrderModel();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_CheckpointOrders_From_The_DB()
        {
            int previousSize = CheckpointOrderModel.GetCheckpointOrders(-1).Count;
            List<CheckpointOrder> checkpointOrdersDb = null;

            Given("we insert three checkpointOrders in the database", () =>
            {
                checkpointOrderModel.AddCheckpointOrderDB(-1, 1);
                checkpointOrderModel.AddCheckpointOrderDB(-1, 2);
                checkpointOrderModel.AddCheckpointOrderDB(-1, 3);
            });

            When("we fetch all checkpointOrders", () =>
            {
                checkpointOrdersDb = CheckpointOrderModel.GetCheckpointOrders(-1);
                LogWriter.getInstance().Write("Received checkpointsDb2: " + checkpointOrdersDb);
            });

            Then("we should have a list of checkpoints", () =>
            {
                CheckpointOrderModel.GetCheckpointOrders(-1).Count().ShouldNotBe(0);
            });
           
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Insert_A_New_CheckpointOrder_To_The_Database()
        {
            int previousSize = CheckpointOrderModel.GetCheckpointOrders(-1).Count;
            
            Given("we want to insert a new checkpointOrder to the database");

            When("we create the checkpointOrder", () =>
            {
                checkpointOrderModel.AddCheckpointOrderDB(-1, 1);                
            });

            Then("it should exist in the database", () =>
            {
                CheckpointOrderModel.GetCheckpointOrders(-1).Count.ShouldNotBe(0);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Update_A_CheckpointOrder_To_The_Database()
        {
            CheckpointOrder origCheckpointOrder = new CheckpointOrder();
            origCheckpointOrder.CheckpointID = 1;
            origCheckpointOrder.StartingNumber = 1;

            CheckpointOrder updatedCheckpointOrder = origCheckpointOrder;

            Given("we have a checkpointOrder", () =>
            {
                checkpointOrderModel.AddCheckpointOrderDB((int)origCheckpointOrder.CheckpointID, (int)origCheckpointOrder.StartingNumber);
            });

            When("we update the checkpointOrder", () =>
            {
                updatedCheckpointOrder.CheckpointID = 1;
                updatedCheckpointOrder.StartingNumber = 2;
                checkpointOrderModel.UpdateCheckpointOrderDB((int)updatedCheckpointOrder.CheckpointID, (int)updatedCheckpointOrder.StartingNumber);
            });

            Then("the values should be different", () =>
            {
                updatedCheckpointOrder.StartingNumber.ShouldNotBe(origCheckpointOrder.StartingNumber);
            });
        }

        [TestMethod]
        public void Get_Next_OrderNumber_In_Ascending_Order_After_Inserting_StartNumber()
        {
            CheckpointOrderModel testCheckpointOrder = null;
            int ordernum1 = 0, ordernum2 = 0;
            int checkpointId = 1;
            Entities context = new Entities();
            Given("We have a CheckpointOrder", () =>
            {
                testCheckpointOrder = new CheckpointOrderModel();
            });

            When("We insert start number", () =>
            {
                testCheckpointOrder.AddCheckpointOrderDB(checkpointId, 2);
                ordernum1 = (int)(context.CheckpointOrders.OrderByDescending(ordnum => ordnum.OrderNumber).First().OrderNumber);
            });

            Then("We should have next order number in ascending order in database", () =>
            {
                testCheckpointOrder.AddCheckpointOrderDB(checkpointId, 3);
                ordernum2 = (int)(context.CheckpointOrders.OrderByDescending(ordnum => ordnum.OrderNumber).First().OrderNumber);
                ordernum2.ShouldBe(ordernum1 + 1);
            });
        }

        [TestMethod]
        public void StartNumber_Should_Be_Added_In_Database()
        {
            CheckpointOrderModel testCheckpointOrder = null;
            int checkpointId = 1;
            int startingNumber = 4;
            Given("We have CheckpointOrder model ", () =>
            {
                testCheckpointOrder = new CheckpointOrderModel();
            });

            When("We insert a new start number", () =>
            {
                testCheckpointOrder.AddCheckpointOrderDB(checkpointId, startingNumber);
            });

            Then("Start Number should be saved in database", () =>
            {
                Entities contextDB = new Entities();
                int startNum = contextDB.CheckpointOrders.Where
                    (chkpntid => (chkpntid.CheckpointID == checkpointId && chkpntid.StartingNumber == startingNumber)).
                    Select(startnum => startnum.StartingNumber).First().Value;
                startNum.ShouldBe(startingNumber);
            });
        }

        [TestMethod]
        public void Deleting_A_CheckpointOrder_Should_Reduce_The_CheckpointOrderList_By_1()
        {
            int initialCheckpointorderCount = 0;

            Given("we add two runtimes to the timer", () =>
            {
                checkpointOrderModel.AddCheckpointOrderDB(timer.GetFirstCheckpointId(), 1);
                checkpointOrderModel.AddCheckpointOrderDB(timer.GetFirstCheckpointId(), 2);
                initialCheckpointorderCount = CheckpointOrderModel.GetCheckpointOrders(timer.GetFirstCheckpointId()).Count;
            });

            When("we want to delete a runtime", () =>
            {
                CheckpointOrderModel.DeleteCheckpointOrder(CheckpointOrderModel.GetCheckpointOrders(timer.GetFirstCheckpointId()).First().ID);
            });

            Then("the checkpointorderlist should be reduced with 1", () =>
            {
                CheckpointOrderModel.GetCheckpointOrders(timer.GetFirstCheckpointId()).Count.ShouldBe(initialCheckpointorderCount - 1);
            });
        }

    }
}
