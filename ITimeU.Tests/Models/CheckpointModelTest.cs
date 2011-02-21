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

        /// TODO:
        /// - When starting a timer with a given checkpoints, its timings should be associated with that
        /// checkpoint. I.e. when calling checkpoint.timings, we should get the start time, runtimes and
        /// end time for that checkpoint. Splittng, we test for the following:
        ///     - When starting the timer, the checkpoint should have a timer with the correct start time.
        ///     - When stopping the timer, the checkpoint should have a timer with the correct end time.
        ///     - When recording runtimes, the checkpoint should have a timer with the correct runtimes.
        /// - The timer's checkpoint should be saved to the DB
        /// - When clicking "Mellomtid", the runtime should be stored for that timer.
        /// - I.e.: Different timers should have different
        /// 
        /// - Checkpoint names should be unique

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_Checkpoints_From_The_Database()
        {
            int previousSize = CheckpointModel.getAll().Count;
            List<CheckpointModel> checkpointsDb = null;

            Given("we insert three checkpoints in the datbase", () =>
            {
                CheckpointModel.Create("First checkpoint");
                CheckpointModel.Create("Second checkpoint");
                CheckpointModel.Create("Third checkpoint");
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

        [TestMethod]
        public void The_Checkpoint_Should_Have_A_Timer_With_Correct_Start_Time_When_Starting_The_Timer()
        {
            TimerModel timer = null;
            CheckpointModel checkpoint = null;
            Given("we have a timer which is associated with a checkpoint", () =>
            {
                timer = new TimerModel();
                checkpoint = CheckpointModel.Create("Supercheckpoint", timer);
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
                checkpoint.Timer.ShouldBe(timer);
            });

        }

        [TestMethod]
        public void A_Checkpoint_Should_Be_Able_To_Have_A_Timer_Db()
        {
            TimerModel timer = null;
            CheckpointModel checkpoint = null;

            Given("we have a timer", () =>
            {
                timer = new TimerModel();
            });

            When("when we create a checkpoint and associate it with a timer", () =>
            {
                checkpoint = CheckpointModel.Create("Supercheckpoint", timer);
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

            Given("we have a checkpoint", () =>
            {
                checkpoint = CheckpointModel.Create("MyCheckpoint");
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


    }
}
