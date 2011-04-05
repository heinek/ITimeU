using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for ResultsControllerTest
    /// </summary>
    [TestClass]
    public class ResultsControllerTest : ScenarioClass
    {
        ResultsController resultController;

        RaceModel race;
        TimerModel timer;
        CheckpointModel checkpoint;
        CheckpointOrderModel checkpointorder;
        TimeStartnumberModel timestart;

        [TestInitialize]
        public void TestStartup()
        {
            Setup();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            //Teardown();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_The_Results()
        {
            ContentResult result = null;
            Given("we have a resultscontroller", () =>
            {
                resultController = new ResultsController();
                resultController.Index();

            });

            When("we approve the raceintermediates", () =>
            {
                result = (ContentResult)resultController.Approve(checkpoint.Id);

            });

            Then("We should get a list of results", () =>
            {
                result.ShouldNotBeNull();
            });
        }
        /// <summary>
        /// Creates the new timer model with checkpoints.
        /// </summary>
        /// <returns></returns>
        private void Setup()
        {
            race = new RaceModel("Testrace", DateTime.Today);
            race.Save();
            timer = new TimerModel();
            timer.RaceID = race.RaceId;
            checkpoint = new CheckpointModel("Checkpoint1", timer, race, 1);
            checkpoint.SaveToDb();
            checkpointorder = new CheckpointOrderModel();
            checkpointorder.AddCheckpointOrderDB(checkpoint.Id, 12);
            checkpointorder.StartingNumber = 12;
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            timer.SaveToDb();
            timestart = new TimeStartnumberModel(timer);
            timestart.CheckpointOrder = checkpointorder;
            //timestart.AddStartnumber(checkpoint.Id, checkpointorder.StartingNumber, 500);
        }

        private void Teardown()
        {
            timestart.DeleteRaceintermediate(checkpoint.Id, checkpointorder.ID);
            checkpoint.Delete();
            timer.Delete();
            race.Delete();
        }
    }
}
