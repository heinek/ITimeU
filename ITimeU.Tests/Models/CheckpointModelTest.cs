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

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// TODO:
    /// - It should be possible to fetch a list of checkpoints from the database
    /// - Insert checkpoint to database
    /// - Gethashcode
    /// - Equals
    /// </summary>
    [TestClass]
    public class CheckpointModelTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        /*
        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_Checkpoints_From_The_Database()
        {
            Given("We have ");
        }
        */

        [TestMethod]
        public void It_Should_Be_Possible_To_Insert_A_New_Checkpoint_To_The_Database()
        {
            string checkpointName = "MyCheckpoint";
            CheckpointModel newCheckpoint = null;

            Given("we want to insert a new checkpoint to the database");

            When("we create the checkpoint", () =>
            {
                newCheckpoint = CheckpointModel.Create(checkpointName);
            });

            Then("it should exist in the database", () =>
            {
                CheckpointModel checkpointDb = CheckpointModel.getById(newCheckpoint.Id);
                checkpointDb.ShouldBe(newCheckpoint);
            });
        }

    }
}
