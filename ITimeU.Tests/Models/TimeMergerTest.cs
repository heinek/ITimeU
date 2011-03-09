using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimeMergerTest
    /// </summary>
    [TestClass]
    public class TimeMergerTest : ScenarioClass
    {
        private TimeMergerModel timeMerger;
        private TimerModel timer;
        private CheckpointOrderModel checkpointOrderModel;
        private CheckpointModel checkpoint1;
        private CheckpointModel checkpoint2;
        [TestInitialize]
        public void TestSetup()
        {
            timeMerger = new TimeMergerModel();
            timer = new TimerModel();
            checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpoint();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            checkpointOrderModel = new CheckpointOrderModel();
        }

        [TestMethod]
        public void Merging_A_CheckpointOrder_And_A_Timestamp_Should_Save_To_Database()
        {
            var raceIntermediate = new RaceIntermediateModel();
            Given("we have a timemerger, a checkpointorder and a timestamp and we add a startnumber and timestamp", () =>
            {
                timer.AddRuntime(400, checkpoint1.Id);
                checkpointOrderModel.AddCheckpointOrderDB(checkpoint1.Id, 1);
            });

            When("we want to merge a checkpointorder with a timestamp", () =>
            {
                raceIntermediate = TimeMergerModel.Merge(checkpoint1.Id, checkpointOrderModel.CheckpointOrderDic.First().Key, timer.CheckpointRuntimes[checkpoint1.Id].First().Key);
            });

            Then("the checkpointorder and timestamp should be merged and saved to database", () => raceIntermediate.ShouldNotBeNull());
        }

        [TestMethod]
        public void A_Merged_CheckpointOrder_And_Timestamp_Should_Be_Fetched_From_To_Database()
        {
            var raceIntermediate = new RaceIntermediateModel();
            Given("we have a timemerger, a checkpointorder and a timestamp and we add a startnumber and timestamp", () =>
            {
                timer.AddRuntime(400, checkpoint1.Id);
                checkpointOrderModel.AddCheckpointOrderDB(checkpoint1.Id, 1);
            });

            When("we want to merge a checkpointorder with a timestamp", () =>
            {
                raceIntermediate = TimeMergerModel.Merge(checkpoint1.Id, checkpointOrderModel.CheckpointOrderDic.First().Key, timer.CheckpointRuntimes[checkpoint1.Id].First().Key);
            });

            Then("the we should be able to retrieve the merged item from the database", () => TimeMergerModel.GetMergedList(checkpoint1.Id).First().RuntimeId.ShouldBe(timer.CheckpointRuntimes[checkpoint1.Id].First().Key));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
    }
}
