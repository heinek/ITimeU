using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using System;
using System.Net;
using TinyBDD.Specification.MSTest;
using ITimeU.Controllers;
using System.Web.Routing;
using System.Web.Mvc;

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
            TimerModel timerInView = new TimerModel(447);

            Given("the user has selected a checkpoint", () =>
            {
                //checkpoint = new CheckpointModel("Hemsedal"); // Create a new checkpoint for this test.
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

        private static void setMockSessionFor(TimerController timerCtrl)
        {
            var sessionItems = new System.Web.SessionState.SessionStateItemCollection();
            timerCtrl.SetFakeControllerContext();
        }

    }
}
