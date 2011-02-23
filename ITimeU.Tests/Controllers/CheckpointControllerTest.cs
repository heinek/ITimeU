using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System.IO;
using ITimeU.Models;
using ITimeU.Controllers;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class CheckpointControllerTest : ScenarioClass
    {

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void A_Checkpoint_Must_Have_A_Timer_When_User_Starts_Timer()
        {
            String requestUrl = null;
            CheckpointModel checkpoint = null;

            Given("the user has selected a checkpoint", () =>
            {
                checkpoint = new CheckpointModel("Hemsedal");
                requestUrl = @"http://localhost:54197/Timer?checkpoint_id=" + checkpoint.Id;
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            });

            When("the user starts the timer", () =>
            {
                requestUrl = @"http://localhost:54197/Timer/Start";
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            });

            Then("the checkpoint should be associated with that timer", () =>
            {
                checkpoint.Timer.IsStarted.ShouldBeTrue(); //fails because timer is null
            });
        }


    }
}
