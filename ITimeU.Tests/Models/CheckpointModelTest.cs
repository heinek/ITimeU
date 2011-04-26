using System;
using System.Collections.Generic;
using ITimeU.Logging;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class CheckpointModelTest : ScenarioClass
    {
        private CheckpointModel checkpoint;
        private EventModel eventModel;
        private RaceModel race;
        private TimerModel timer;
        [TestInitialize]
        public void TestSetup()
        {
            timer = new TimerModel();
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("SomeRace", new DateTime(2007, 10, 3));
            race.EventId = eventModel.EventId;
            race.Save();
            checkpoint = new CheckpointModel("Checkpoint1", timer, race, 1);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            timer.SaveToDb();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            checkpoint.Delete();
            timer.Delete();
            race.Delete();
            eventModel.Delete();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_Checkpoints_From_The_Database()
        {
            List<CheckpointModel> checkpointsDb = null;

            int previousSize = CheckpointModel.getAll().Count;
            Given("we insert three checkpoints in the datbase", () =>
            {
                new CheckpointModel("1st checkpoint", timer, race, 1);
                new CheckpointModel("2nd checkpoint", timer, race, 2);
                new CheckpointModel("3rd checkpoint", timer, race, 3);
            });

            When("we fetch all checkpoints", () =>
            {
                checkpointsDb = CheckpointModel.getAll();
                LogWriter.getInstance().Write("Received checkpointsDb2: " + checkpointsDb);
            });

            Then("we should have a list of checkpoints", () =>
            {
                checkpointsDb.Count.ShouldBe(previousSize + 3);
            });

        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Insert_A_New_Checkpoint_To_The_Database()
        {
            CheckpointModel newCheckpoint = null;
            Given("we want to insert a new checkpoint to the database");

            When("we create the checkpoint", () =>
            {
                newCheckpoint = new CheckpointModel("MyCheckpoint", timer, race, 1);
                newCheckpoint = new CheckpointModel("MyCheckpoint", new TimerModel(), race, 1);
            });

            Then("it should exist in the database", () =>
            {
                CheckpointModel checkpointDb = CheckpointModel.getById(newCheckpoint.Id);
                checkpointDb.ShouldBe(newCheckpoint);
            });
        }

        [TestMethod]
        public void The_Checkpoint_Should_Have_A_Timer_With_Correct_Start_Time_When_Starting_The_Timer()
        {
            Given("we have a timer which is associated with a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("RelationToTimerCheckpoint", timer, race);
            });

            When("we start the timer", () => timer.Start());

            Then("the checkpoint should have a timer with the correct start time", () =>
            {
                CheckpointModel checkpointDb = CheckpointModel.getById(checkpoint.Id);
                checkpointDb.Timer.StartTime.ShouldBe(timer.StartTime);
            });
        }

        [TestMethod]
        public void A_Checkpoint_Should_Be_Able_To_Have_A_Timer()
        {
            Given("we have a timer", () =>
            {
            });

            When("when we create a checkpoint and associate it with a timer", () =>
            {
                checkpoint = new CheckpointModel("Supercheckpoint", timer, race);
            });

            Then("the checkpoint should have the correct timer associated with it", () =>
            {
                CheckpointModel checkpointDb = CheckpointModel.getById(checkpoint.Id);
                checkpointDb.Timer.ShouldBe(timer);
            });
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Insert_And_Fetch_A_Checkpoint_To_Database()
        {
            CheckpointModel checkpointDb = null;

            Given("we have a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("MyCheckpoint", timer, race, 1);
            });

            When("we fetch the same checkpoint from database", () =>
            {
                checkpointDb = CheckpointModel.getById(checkpoint.Id);
            });

            Then("the checkpoints should be the same", () =>
            {
                checkpointDb.ShouldBe(checkpoint);
            });
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Insert_And_Fetch_A_Checkpoint_With_A_Race_To_Database()
        {
            CheckpointModel checkpointDb = null;
            TimerModel timer = new TimerModel();
            timer.SaveToDb();

            Given("we have a checkpoint in the database", () =>
            {
            });

            When("we fetch the same checkpoint from database", () =>
            {
                checkpointDb = CheckpointModel.getById(checkpoint.Id);
            });

            Then("the checkpoints should be the same", () =>
            {
                checkpointDb.Name.ShouldBe(checkpoint.Name);
                checkpointDb.Race.RaceId.ShouldBe(checkpoint.Race.RaceId);
            });
        }
    }
}
