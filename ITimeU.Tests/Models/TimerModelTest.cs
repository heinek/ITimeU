using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
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
        private RaceModel race = null;

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
                catch (InvalidOperationException)
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
        public void The_Start_Time_Should_Be_Set_When_Started()
        {
            Given("we have a timer", () => timer = new TimerModel());

            When("we start the timer", () => timer.Start());

            Then("the start time should be saved to the database", () =>
            {
                timer.StartTime.HasValue.ShouldBeTrue();
            });
        }

        [TestMethod]
        public void The_End_Time_Should_Be_Saved_To_The_Database()
        {
            // Known bug in this test. See doc/KnownBugs.txt.

            DateTime endTime = new DateTime();

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop the timer", () =>
            {
                timer.Stop();
                endTime = (DateTime)timer.EndTime;
            });

            Then("the end time should be saved to the database", () =>
            {
                int year = endTime.Year;
                int month = endTime.Month;
                int day = endTime.Day;
                int hour = endTime.Hour;
                int min = endTime.Minute;
                int sec = endTime.Second;
                int millisec = endTime.Millisecond;

                var timerDb = TimerModel.GetTimerById(timer.Id);
                var endTimeDb = (DateTime) timerDb.EndTime;

                endTimeDb.Year.ShouldBe(year);
                endTimeDb.Month.ShouldBe(month);
                endTimeDb.Day.ShouldBe(day);
                endTimeDb.Minute.ShouldBe(min);
                endTimeDb.Second.ShouldBe(sec);
                endTimeDb.Millisecond.ShouldBe(millisec);
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
        public void The_Start_Time_Should_Be_Null_When_Reset_Is_Pushed()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop and reset the timer", () =>
            {
                timer.Stop();
                timer.Reset();
            });

            Then("the start time should be set to null", () =>
            {
                timer.StartTime.ShouldBeNull();
            });
        }

        [TestMethod]
        public void The_Endtime_Should_Be_Null_When_Reset_Is_Pushed()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop and reset the timer", () =>
            {
                timer.Stop();
                timer.Reset();
            });

            Then("the end time should be set to null", () =>
            {
                timer.EndTime.ShouldBeNull();
            });
        }

        [TestMethod]
        public void A_New_TimerModel_Should_Be_Created_When_We_Reset_The_Timer()
        {
            int timerId = 0;
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
                timerId = timer.Id;
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Reset();
            });

            Then("a new timer should be created", () =>
            {
                timer.Id.ShouldNotBe(timerId);
            });
        }

        [TestMethod]
        public void The_Start_Time_Should_Be_Set_When_We_Reset_And_Start_The_Timer()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Reset();
                timer.Start();
            });

            Then("the start time should be set", () =>
            {
                timer.StartTime.ShouldNotBeNull();
            });
        }

        [TestMethod]
        public void Resetting_The_Timer_When_It_Is_Started_Should_Not_Be_Allowed()
        {
            // Reason: The GUI button "Nullstill" (eng: Reset) is disabled when the
            // timer is started. This test ensures this behavior.

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we reset the timer", () =>
            {
                try
                {
                    timer.Reset();
                    false.ShouldBeTrue();
                } catch (InvalidOperationException) { }
            });

            Then("we should get an exception");
        }

        [TestMethod]
        public void We_Should_Have_A_List_With_Races()
        {
            var races = new List<RaceModel>();
            Given("we have a timer", () => timer = new TimerModel());

            When("we want to select a race", () => races = RaceModel.GetRaces());

            Then("the racelist should contain at least one race", () => races.Count.ShouldNotBe(0));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

    }
}
