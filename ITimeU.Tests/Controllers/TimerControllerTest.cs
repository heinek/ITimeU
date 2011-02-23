using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using System;
using System.Net;
using TinyBDD.Specification.MSTest;
using ITimeU.Controllers;
using System.Web.Routing;

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

            Given("the user has selected a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("Hemsedal");

                // Execute controller
                timerCtrl = new TimerController();
                timerCtrl.Index(checkpoint.Id);
                
                /*
                // Execute controller
                requestUrl = @"http://localhost:54197/Timer?checkpoint_id=" + checkpoint.Id;
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpRequest.GetResponse().Close();
                */
            });

            When("the user starts the timer", () =>
            {
                timerCtrl.Start();
                /*
                requestUrl = @"http://localhost:54197/Timer/Start";
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpRequest.GetResponse().Close();
                */
            });

            Then("the checkpoint should be associated with that timer", () =>
            {
                checkpoint.Timer.IsStarted.ShouldBeTrue(); //fails because timer is null
            });
        }

    }
}
