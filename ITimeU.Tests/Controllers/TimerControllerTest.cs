using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System.Collections.Generic;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for TimerControllerTest
    /// </summary>
    [TestClass]
    public class TimerControllerTest : ScenarioClass
    {

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void A_Checkpoint_Must_Have_A_Timer_When_User_Starts_Timer()
        {
            TimerController timerCtrl = null;
            CheckpointModel checkpoint = null;
            var timerInView = CreateTimerWithCheckpoints();

            Given("the user has selected a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("Hemsedal", timerInView, 1);
                timerCtrl = new TimerController();
                setMockSessionFor(timerCtrl);
            });

            When("the user selects a checkpoint and clicks OK", () =>
            {
                ViewResult ctrlResult = (ViewResult)timerCtrl.Index(timerInView.Id);
                timerInView = (TimerModel)ctrlResult.Model;
            });

            Then("the checkpoint's timer should be associated with the timer in the view", () =>
            {
                // We currently have to re-fetch the checkpoint from database, because the TimerController
                // updates its own instance of the CheckpointModel, not the checkpoint instance we're using
                // here.
                CheckpointModel checkpointDb = CheckpointModel.getById(checkpoint.Id);
                timerInView.ShouldBe(checkpointDb.Timer);
            });
        }

        private TimerModel CreateTimerWithCheckpoints()
        {
            var timer = new TimerModel();
            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }

        private static void setMockSessionFor(TimerController timerCtrl)
        {
            var sessionItems = new System.Web.SessionState.SessionStateItemCollection();
            timerCtrl.SetFakeControllerContext();
        }

    }
}
