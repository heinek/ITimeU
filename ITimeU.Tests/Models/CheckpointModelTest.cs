using System.Collections.Generic;
using ITimeU.Logging;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class CheckpointModelTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_Checkpoints_From_The_Database()
        {
            List<CheckpointModel> checkpointsDb = null;
            var timer = CreateNewTimerModelWithCheckpoints();
            int previousSize = CheckpointModel.getAll().Count;
            Given("we insert three checkpoints in the datbase", () =>
            {
                new CheckpointModel("1st checkpoint", timer, 1);
                new CheckpointModel("2nd checkpoint", timer, 2);
                new CheckpointModel("3rd checkpoint", timer, 3);
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
                newCheckpoint = new CheckpointModel("MyCheckpoint", new TimerModel(), 1);
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
            TimerModel timer = null;
            CheckpointModel checkpoint = null;
            Given("we have a timer which is associated with a checkpoint", () =>
            {
                timer = new TimerModel();
                checkpoint = new CheckpointModel("RelationToTimerCheckpoint", timer);
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
            TimerModel timer = null;
            CheckpointModel checkpoint = null;

            Given("we have a timer", () =>
            {
                timer = new TimerModel();
            });

            When("when we create a checkpoint and associate it with a timer", () =>
            {
                checkpoint = new CheckpointModel("Supercheckpoint", timer);
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
            CheckpointModel checkpoint = null;
            CheckpointModel checkpointDb = null;
            var timer = CreateNewTimerModelWithCheckpoints();
            Given("we have a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("MyCheckpoint", timer, 1);
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

        private TimerModel CreateNewTimerModelWithCheckpoints()
        {
            var timer = new TimerModel();
            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Insert_And_Fetch_A_Checkpoint_With_A_Race_To_Database()
        {
            CheckpointModel checkpoint = null;
            CheckpointModel checkpointDb = null;
            RaceModel race = new RaceModel("Malvikløpet", new System.DateTime(2011, 3, 2));
            race.Save(); // We assume that the race is stored in the database already.

            Given("we have a checkpoint in the database", () =>
            {
                checkpoint = new CheckpointModel("MalvikCheckpoint", race.RaceId);
                checkpoint.SaveToDb();
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
