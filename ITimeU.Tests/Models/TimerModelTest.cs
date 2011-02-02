using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;
using System.Threading;
using ITimeU.Controllers;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimerModelTest
    /// </summary>
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {

        private static TimerModel timerModel = null;

        [TestMethod]
        public void We_Have_A_Timer_Model()
        {
            Scenario("We have a timer model");
            Given("we want to create a timer model");

            When("we instantiate the timer model class",
                () => timerModel = new TimerModel()
            );

            Then("we have a timer model", () => timerModel.ShouldNotBeNull()
                /* Assert.IsNotNull(timerModel)*/
            );
        }

        [TestMethod]
        public void The_TimerModel_Has_A_Starttime()
        {
            Given("we have an instance of the timerclass", () => timerModel = new TimerModel());

            When("we we click the startbutton", () => timerModel.Start());

            Then("the timer should have a starttime", () => Assert.IsNotNull(timerModel.StartTime));
        }

        [TestMethod]
        public void Start_Time_Should_Return_Same_Value()
        {
            Object startTime = null;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());

            When("We click the start button", () =>
            {
                timerModel.Start();
                startTime = timerModel.StartTime;
                Thread.Sleep(10);
            });


            Then("THe timer should return the same value each time", () => Assert.AreEqual(startTime, timerModel.StartTime));
        }

        [TestMethod]
        public void We_Should_Have_A_View_With_A_Start_Button()
        {
            Given("we have an instance of timerclass", () => timerModel = new TimerModel());

            When("we want to start the timer");

            Then("a view with a timer should appear", () =>
            {
                TimerController timerController = new TimerController();
                ViewResult result = (ViewResult) timerController.Index();
                result.ViewName.ShouldBe("Index");
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
