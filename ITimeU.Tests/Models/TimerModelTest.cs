using System;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.DAL;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {
        private TimerModel timer = null;

        [TestMethod]
        public void A_Timer_Exists()
        {
            Given("we want to create a timer");

            When("we instantiate the timer",
                () => timer = new TimerModel()
            );

            Then("we have a timer", () => timer.ShouldNotBeNull()
            );
        }

        [TestMethod]
        public void A_Timer_Has_A_Starttime()
        {
            Given("we have an instance of the timerclass", () => timer = new TimerModel());
            When("we we click the startbutton", () => timer.Start());
            Then("the timer should have a starttime", () => Assert.IsNotNull(timer.StartTime));
        }

        [TestMethod]
        public void A_Started_Timer_Should_Always_Return_The_Same_StartTime()
        {
            DateTime? startTime = null;
            Given("we have a timer", () => timer = new TimerModel());

            When("we click the start button", () =>
            {
                timer.Start();
                startTime = timer.StartTime;
                Thread.Sleep(100);
            });

            Then("the timer should return the same start time every time", () =>
                startTime.ShouldBe(timer.StartTime)
            );
        }

        [TestMethod]
        public void The_TimerController_Should_Have_A_View_Named_Index()
        {
            TimerController timerController = null;

            Given("we have a timer", () => timer = new TimerModel());

            When("we want to start the timer", () =>
            {
                timerController = new TimerController();
                timerController.SetFakeControllerContext();
            });

            Then("a view with a timer should appear", () =>
            {
                ViewResult result = (ViewResult)timerController.Index();
                result.ViewName.ShouldBe("Index");
            });
        }

        [TestMethod]
        public void Starting_A_Timer_Twice_Should_Throw_Exception()
        {
            Given("we have a timer", () => timer = new TimerModel());

            When("we start the timer twice", () =>
            {
                timer.Start();
                try
                {
                    timer.Start();
                    false.ShouldBeTrue(); // Fail test, we shouldn't get here.
                }
                catch (InvalidOperationException e)
                {
                    true.ShouldBeTrue();
                }
            });

            Then("we should get an exception");
        }

        [TestMethod]
        public void The_Start_Time_Should_Be_Null_Before_Timer_Is_Started()
        {
            DateTime? startTime = new DateTime();

            Given("we have a timer", () => timer = new TimerModel());
            When("we fetch the start time", () => startTime = timer.StartTime);
            Then("the start time should be null", () => startTime.ShouldBeNull());
        }

        [TestMethod]
        public void The_Timer_Should_Initially_Not_Be_Started()
        {
            Given("we are going to create a timer");
            When("we create the timer", () => timer = new TimerModel());
            Then("the timer should not be started", () => timer.IsStarted.ShouldBeFalse());
        }

        [TestMethod]
        public void The_Start_Time_Should_Be_Saved_To_The_Database()
        {
            DateTime? startTime = new DateTime();

            Given("we have a timer", () => timer = new TimerModel());

            When("we start the timer", () =>
            {
                timer.Start();
                startTime = timer.StartTime;
            });

            Then("the start time should be saved to the database", () =>
            {
                var timerDal = TimerDAL.GetTimerById(timer.Id);
                timerDal.StartTime.HasValue.ShouldBeTrue();
            });
        }

        [TestMethod]
        public void The_End_Time_Should_Be_Saved_To_The_Database()
        {
            DateTime? startTime = new DateTime();

            Given("we have a started timer", () => {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop the timer", () =>
            {
                timer.Stop();
                startTime = timer.EndTime;
            });

            Then("the end time should be saved to the database", () =>
            {
                var timerDal = TimerDAL.GetTimerById(timer.Id);
                timerDal.EndTime.HasValue.ShouldBeTrue();
            });
        }
        [TestMethod]
        public void The_Running_Time_Should_Be_Shown()
        {
            Given("we have a timer", () => timer = new TimerModel());

            When("the timer is started", () => timer.Start());

            Then("the running time should have a value", () =>
            {
                TimerController timerController = new TimerController();
                timerController.SetFakeControllerContext();
                ViewResult result = (ViewResult)timerController.Index();
                result.ViewName.ShouldBe("Index");
                result.ViewData["time1"].ShouldNotBeNull();
            });
        }

        [TestMethod]
        public void A_Timer_Should_Not_Be_Running_After_It_Has_Stopped()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("the timer is stopped", () => timer.Stop());

            Then("the timer should be stopped", () =>
            {
                timer.IsStarted.ShouldBeFalse();
            });
        }

        [TestMethod]
        public void A_Stopped_Timer_Must_Have_A_Start_Time()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("the timer is stopped", () => timer.Stop());

            Then("the timer should have a start time", () =>
            {
                timer.StartTime.ShouldBeInstanceOfType<DateTime>();
            });
        }

        // TODO: A Stopped timer must have an end time
        [TestMethod]
        public void A_Stopped_Timer_Must_Have_An_End_Time()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("the timer is stopped", () => timer.Stop());

            Then("the timer should have an end time", () =>
            {
                timer.EndTime.ShouldBeInstanceOfType<DateTime>();
            });
        }

        [TestMethod]
        public void The_start_Time_Should_Be_Stoped()
        {
            DateTime stopTime = new DateTime();

            Given("We have a start timer", () =>
            {
                timerModel = new TimerModel();
                timerModel.Start();
            }
                );

            When("We stop the time", () =>
            {
                timerModel.Stop();
            }
                );

            Then("The start time should be stopped", () =>
                {
                    var time = TimerDAL.GetTimerById(timerModel.Id);
                    timerModel.EndTime.HasValue.ShouldBeTrue();
                }
                );
        }

        [TestMethod]
        public void The_Starttime_Should_Be_Null_When_Reset_Is_Pushed()
        {
            Given("we have a starttimer", () =>
                {
                    timerModel = new TimerModel();
                    timerModel.Start();
                });

            When("we stop and restart the timer", () =>
            {
                timerModel.Stop();
                timerModel.Restart();
            });

            Then("the timer should be set to 0", () =>
            {
                timerModel.StartTime.ShouldBeNull();
            });
        }

        [TestMethod]
        public void The_Endtime_Should_Be_Null_When_Reset_Is_Pushed()
        {
            Given("we have a starttimer", () =>
            {
                timerModel = new TimerModel();
                timerModel.Start();
            });

            When("we stop and restart the timer", () =>
            {
                timerModel.Stop();
                timerModel.Restart();
            });

            Then("the timer should be set to 0", () =>
            {
                timerModel.EndTime.ShouldBeNull();
            });
        }

        [TestMethod]
        public void A_New_TimerModel_Should_Be_Created_When_We_Reset_The_Timer()
        {
            int timerModelId = 0;
            Given("we have a starttimer", () =>
            {
                timerModel = new TimerModel();
                timerModel.Start();
                timerModelId = timerModel.Id;
            });

            When("we stop and restart the timer", () =>
            {
                timerModel.Stop();
                timerModel.Restart();
            });

            Then("a new timer should be created", () =>
            {
                timerModel.Id.ShouldNotBe(timerModelId);
            });
        }

        [TestMethod]
        public void Starttime_Should_Be_Saved_When_We_Start_A_New_Timer_After_Reset()
        {
            int timerModelId = 0;
            Given("we have a starttimer", () =>
            {
                timerModel = new TimerModel();
                timerModel.Start();
                timerModelId = timerModel.Id;
            });

            When("we stop and restart the timer", () =>
            {
                timerModel.Stop();
                timerModel.Restart();
                timerModel.Start();
            });

            Then("a new timer should be created", () =>
            {
                timerModel.StartTime.ShouldNotBeNull();
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
