using System;
using System.Collections.Generic;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;


namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for RaceIntermediateModelTest
    /// </summary>
    [TestClass]
    public class RaceIntermediateModelTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
        private RaceModel race;
        private TimeStartnumberModel timestartnumberModel;
        private TimeMergerModel timeMerger;
        private TimerModel timer;
        private CheckpointOrderModel checkpointOrderModel;
        private CheckpointModel checkpoint1;
        private CheckpointModel checkpoint2;

        [TestInitialize]
        public void TestSetup()
        {
            race = new RaceModel("TestLøp", DateTime.Today);
            race.Save();
            timeMerger = new TimeMergerModel();
            timer = new TimerModel();
            timer.RaceID = race.RaceId;
            checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            checkpointOrderModel = new CheckpointOrderModel();
            timestartnumberModel = new TimeStartnumberModel(timer);
            timestartnumberModel.ChangeCheckpoint(timer.GetFirstCheckpointId());
            timestartnumberModel.CheckpointOrder = checkpointOrderModel;
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_All_RaceIntermediates_For_A_Race()
        {
            Given("we have a race with raceintermediates", () =>
            {
                timestartnumberModel.AddStartnumber(timestartnumberModel.CurrentCheckpointId, 10, 400);
            });

            When("we want to see the results");

            Then("we should get all raceintermediates for the race", () =>
            {
                RaceIntermediateModel.GetRaceintermediateForRace(race.RaceId).ShouldNotBeNull();
            });

        }
    }
}
