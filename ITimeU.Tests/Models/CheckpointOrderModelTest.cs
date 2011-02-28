using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;
using ITimeU.Logging;

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
                newCheckpointOrder = CheckpointOrder.CreateCheckpointOrder(1);
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
                origCheckpointOrder = CheckpointOrder.CreateCheckpointOrder(1);
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


    }
}
