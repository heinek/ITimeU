using System.Collections.Generic;
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
        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_CheckpointOrders_From_The_DB()
        {
            int previousSize = CheckpointOrderModel.GetAllCheckpointOrders().Count;
            List<CheckpointOrder> checkpointOrdersDb = null;

            Given("we insert three checkpointOrders in the database", () =>
            {
                //CheckpointOrderModel.Create(1, 1, 1);
                //CheckpointOrderModel.Create(1, 2, 3);
                //CheckpointOrderModel.Create(1, 3, 2);
            });

            When("we fetch all checkpointOrders", () =>
            {
                checkpointOrdersDb = CheckpointOrderModel.GetAllCheckpointOrders();
                LogWriter.getInstance().Write("Received checkpointsDb2: " + checkpointOrdersDb);
            });

            Then("we should have a list of checkpoints", () =>
            {
                checkpointOrdersDb.Count.ShouldBe(previousSize + 3);
            });

        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Insert_A_New_CheckpointOrder_To_The_Database()
        {
            CheckpointOrder newCheckpointOrder = null;

            Given("we want to insert a new checkpointOrder to the database");

            When("we create the checkpointOrder", () =>
            {
                newCheckpointOrder = CheckpointOrder.CreateCheckpointOrder(1, false);
            });

            Then("it should exist in the database", () =>
            {
                CheckpointOrder checkpointOrderDb = CheckpointOrderModel.GetCheckpointOrderById(newCheckpointOrder.ID);
                checkpointOrderDb.ShouldBe(newCheckpointOrder);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Update_A_CheckpointOrder_To_The_Database()
        {
            CheckpointOrder origCheckpointOrder = null;
            CheckpointOrder updatedCheckpointOrder = origCheckpointOrder;

            Given("we have a checkpointOrder", () =>
            {
                origCheckpointOrder = CheckpointOrder.CreateCheckpointOrder(1, false);
                origCheckpointOrder.CheckpointID = 1;
            });

            When("we update the checkpointOrder", () =>
            {
                updatedCheckpointOrder.CheckpointID = 2;
            });

            Then("the values should be different", () =>
            {
                updatedCheckpointOrder.CheckpointID.ShouldNotBeSameAs(origCheckpointOrder.CheckpointID);
            });
        }

        [TestMethod]
        public void Get_Next_OrderNumber_In_Ascending_Order_After_Inserting_StartNumber()
        {
            CheckpointOrderModel testCheckpointOrder = null;
            int ordernum1 = 0, ordernum2 = 0;
            int checkpointId = 1;
            int startingNumber = 2;
            Entities context = new Entities();
            Given("We have a CheckpointOrder", () =>
                {
                    testCheckpointOrder = new CheckpointOrderModel();
                    
                });

            When("We insert start number", () =>
                {
                    testCheckpointOrder.AddCheckpointOrderDB(checkpointId, startingNumber);
                    ordernum1 = (int)(context.CheckpointOrders.OrderByDescending(ordnum => ordnum.OrderNumber).First().OrderNumber);
                });

            Then("We should have next order number in ascending order in database", () =>
                {
                    startingNumber = 3;
                    testCheckpointOrder.AddCheckpointOrderDB(checkpointId, startingNumber);
                    ordernum2 = (int)(context.CheckpointOrders.OrderByDescending(ordnum => ordnum.OrderNumber).First().OrderNumber);
                    ordernum2.ShouldBeSameAs(ordernum1+1);
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
                    var startNum = contextDB.CheckpointOrders.Where
                        (chkpntid => (chkpntid.CheckpointID == checkpointId && chkpntid.StartingNumber == startingNumber)).
                        Select(startnum => startnum.StartingNumber);
                    startNum.ShouldBe(startingNumber);
                });
        }


    }
}
