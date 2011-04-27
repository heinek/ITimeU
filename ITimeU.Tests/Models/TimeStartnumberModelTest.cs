using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimeStartnumberModelTest
    /// </summary>
    [TestClass]
    public class TimeStartnumberModelTest : ScenarioClass
    {
        private TimeStartnumberModel timestartnumberModel;
        private TimeMergerModel timeMerger;
        private TimerModel timer;
        private CheckpointOrderModel checkpointOrderModel;
        private CheckpointModel checkpoint1;
        private CheckpointModel checkpoint2;
        private EventModel eventModel;
        private RaceModel race;

        [TestInitialize]
        public void TestSetup()
        {
            timeMerger = new TimeMergerModel();
            timer = new TimerModel();
            eventModel = new EventModel("Testevent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("SomeRace", new DateTime(2007, 10, 3));
            race.EventId = eventModel.EventId;
            race.Save();
            checkpoint1 = new CheckpointModel("Checkpoint1", timer, race, 1);
            checkpoint2 = new CheckpointModel("Checkpoint2", timer, race, 2);
            timer.RaceID = race.RaceId;
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            checkpointOrderModel = new CheckpointOrderModel();
            timestartnumberModel = new TimeStartnumberModel(timer);
            timestartnumberModel.ChangeCheckpoint(timer.GetFirstCheckpointId());
            timestartnumberModel.CheckpointOrder = checkpointOrderModel;
        }

        [TestMethod]
        public void Changing_A_Checkpoint_Should_Update_CurrentcheckpointId()
        {
            var initialcheckpointId = timestartnumberModel.CurrentCheckpointId;
            Given("we have a timestartnumber registrerer");
            When("we want to change the checkpoint", () =>
            {
                timestartnumberModel.ChangeCheckpoint(checkpoint2.Id);
            });

            Then("the current checkpointid should be different from the initial", () =>
            {
                timestartnumberModel.CurrentCheckpointId.ShouldNotBeSameAs(initialcheckpointId);
            });
        }

        [TestMethod]
        public void Changing_A_Checkpoint_Should_Change_The_Raceintermediates()
        {
            var raceintermediates = timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId];
            Given("we have a timestartnumber registrerer");
            When("we want to change the checkpoint", () =>
            {
                timestartnumberModel.ChangeCheckpoint(checkpoint2.Id);
            });

            Then("the current checkpointid should be different from the initial", () =>
            {
                timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId].ShouldNotBeSameAs(raceintermediates);
            });
        }

        [TestMethod]
        public void Adding_A_Startnumber_Should_Increase_The_Raceintermediates_With_1()
        {
            var initialcount = timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId].Count;
            Given("we have a timestartnumber registrerer");

            When("we add a new startnumber", () =>
            {
                timestartnumberModel.AddStartnumber(timestartnumberModel.CurrentCheckpointId, 3, 4500);
            });

            Then("the count should increase by 1", () =>
            {
                timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId].Count.ShouldBe(initialcount + 1);
            });
        }

        [TestMethod]
        public void Editing_A_Runtime_Should_Update_The_New_Raceintermediate()
        {
            var initialvalue = new TimeSpan(0, 0, 0, 2, 0);
            var newvalue = new TimeSpan(0, 0, 0, 3, 0);

            Given("we have a timestartnumber registrerer and add a new startnumber and runtime", () =>
            {
                timestartnumberModel.AddStartnumber(timestartnumberModel.CurrentCheckpointId, 3, (int)initialvalue.TotalMilliseconds);
            });

            When("we want to edit that runtime", () =>
            {
                RuntimeModel.EditRuntime(timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId].First().RuntimeId, newvalue.Hours, newvalue.Minutes, newvalue.Seconds, newvalue.Milliseconds);
            });

            Then("the timestamp should have changed to the new value", () =>
            {
                var runtime = RuntimeModel.getById(timestartnumberModel.CheckpointIntermediates[timestartnumberModel.CurrentCheckpointId].First().RuntimeId);
                runtime.Runtime.ShouldBe((int)newvalue.TotalMilliseconds);
            });
        }

        [TestMethod]
        public void Deleting_A_Raceintermediate_Should_Reduce_The_Raceintermediatelist_With_1()
        {
            var initiallistcount = 0;
            var currentCheckpointId = timestartnumberModel.CurrentCheckpointId;

            Given("we have a timestartnumber registrerer and add a new startnumber and runtime", () =>
            {
                timestartnumberModel.AddStartnumber(timestartnumberModel.CurrentCheckpointId, 3, 4000);
                initiallistcount = timestartnumberModel.CheckpointIntermediates[currentCheckpointId].Count;
            });

            When("we delete a raceintermediate", () =>
            {
                timestartnumberModel.DeleteRaceintermediate(currentCheckpointId, timestartnumberModel.CheckpointIntermediates[currentCheckpointId].First().CheckpointOrderID);
            });

            Then("the listcount should be reduced with 1", () =>
            {
                timestartnumberModel.CheckpointIntermediates[currentCheckpointId].Count.ShouldBe(initiallistcount - 1);
            });
        }

        [TestMethod]
        public void Editing_A_Startnumber_Should_Update_The_New_Raceintermediate()
        {
            var currentCheckpointId = timestartnumberModel.CurrentCheckpointId;
            int newvalue = 4;
            Given("we have a timestartnumber registrerer and add a new startnumber and runtime", () =>
            {
                timestartnumberModel.AddStartnumber(currentCheckpointId, 3, 5000);
            });

            When("we want to edit that startnumber", () =>
            {
                timestartnumberModel.EditStartnumber(currentCheckpointId, timestartnumberModel.CheckpointIntermediates[currentCheckpointId].First().CheckpointOrderID, newvalue);
            });

            Then("the startnumber should have changed to the new value", () =>
            {
                var startnumber = CheckpointOrderModel.GetCheckpointOrderById(timestartnumberModel.CheckpointIntermediates[currentCheckpointId].First().CheckpointOrderID).StartingNumber.Value;

                startnumber.ShouldBe(newvalue);
            });
        }
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            race.Delete();
            eventModel.Delete();
            checkpoint1.Delete();
            checkpoint2.Delete();
            timer.Delete();
        }
    }
}
