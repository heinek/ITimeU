using System;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimerModelTest
    /// </summary>
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {

        private TimerModel timerModel = null;

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
            DateTime startTime = DateTime.MinValue;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());

            When("We click the start button", () =>
            {
                timerModel.Start();
                startTime = timerModel.StartTime;
                Thread.Sleep(100);
            });


            Then("THe timer should return the same value each time", () => startTime.ShouldBe(timerModel.StartTime));
        }

        [TestMethod]
        public void We_Should_Have_A_View_With_A_Start_Button()
        {
            Given("we have an instance of timerclass", () => timerModel = new TimerModel());

            When("we want to start the timer");

            Then("a view with a timer should appear", () =>
            {
                TimerController timerController = new TimerController();
                ViewResult result = (ViewResult)timerController.Index(null);
                result.ViewName.ShouldBe("Index");
            });
        }

        [TestMethod]
        public void Start_Time_Should_Return_Same_Value_Even_when_you_press_start_again()
        {
            Object startTime = null;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());
            When("We click the start button twice", () =>
                {
                    timerModel.Start();
                    startTime = timerModel.StartTime;
                    Thread.Sleep(10);
                    timerModel.Start();
                });
            Then("The timer should return the same value each time", () => startTime.ShouldBe(timerModel.StartTime));

        }

        [TestMethod]
        public void Should_Get_Exception_When_Getting_StartTime_Before_Timer_Is_Started()
        {
            Object startTime = null;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());
            When("We fetch the starttime", () =>
                {
                    try
                    {
                        startTime = timerModel.StartTime;
                        true.ShouldBeFalse();
                    }
                    catch (NullReferenceException e)
                    {
                        true.ShouldBeTrue();
                    }
                });
        }

        [TestMethod]
        public void The_Timer_Should_Initially_Not_Be_Started()
        {
            Given("we are going to create a timer");
            When("we create the timer", () => timerModel = new TimerModel());
            Then("the timer should not be started", () => timerModel.IsStarted.ShouldBeFalse());
        }
        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
