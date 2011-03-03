using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimeMergerTest
    /// </summary>
    [TestClass]
    public class TimeMergerTest : ScenarioClass
    {
        private TimeMergerModel timeMerger;
        private Timer timer;
        private CheckpointOrderModel checkpointOrderModel;
        private CheckpointModel checkpoint1;
        private CheckpointModel checkpoint2;
        //[TestInitialize]
        //public void TestSetup()
        //{
        //    timeMerger = new TimeMergerModel();
        //    timer = new TimerModel();
        //    checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
        //    checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
        //    timer.CurrentCheckpointId = timer.GetFirstCheckpoint();
        //    timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
        //    checkpointOrderModel = new CheckpointOrderModel();
        //    checkpointOrderModel.CheckpointID = checkpoint1.Id;
        //    checkpointOrderModel.OrderNumber = 1;
        //    checkpointOrderModel.StartingNumber = 10;
        //}

        [TestMethod]
        public void Merging_A_CheckpointOrder_And_A_Timestamp_Should_Save_To_Database()
        {
            Given("we have a timemerger, a checkpointorder and a timestamp");

            When("we want to merg a checkpointorder with a timestamp");

            Then("the checkpointorder and timestamp should be merged and saved to database");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
    }
}
