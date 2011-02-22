using System;
using System.Linq;
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
    /// <summary>
    /// TODO: Constructor new TimerModel(Timer) must be created a test for.
    /// </summary>
    
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {
        private TimerModel timer = null;
        private RaceModel race = null;
        private RuntimeModel runtime = null;

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
        public void A_Started_Timer_Must_Have_A_Starttime()
        {
            Given("we have an instance of the timerclass", () => timer = new TimerModel());
            When("we we click the startbutton", () => timer.Start());
            Then("the timer should have a starttime", () => timer.StartTime.ShouldBeInstanceOfType<DateTime>());
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
        public void The_Start_Time_Should_Be_Saved_To_The_Database()
        {
            // Known bug in this test. See doc/KnownBugs.txt.

            DateTime startTime = new DateTime();

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
            });

            When("we start the timer", () =>
            {
                timer.Start();
                startTime = (DateTime)timer.StartTime;
            });

            Then("the start time should be saved to the database", () =>
            {
                // startTimeDb.ShouldBe(startTime) does not work, so have to do it pretty heavy like this.
                int year = startTime.Year;
                int month = startTime.Month;
                int day = startTime.Day;
                int hour = startTime.Hour;
                int min = startTime.Minute;
                int sec = startTime.Second;
                int millisec = startTime.Millisecond;

                var timerDb = TimerModel.GetTimerById(timer.Id);
                var startTimeDb = (DateTime)timerDb.StartTime;

                startTimeDb.Year.ShouldBe(year);
                startTimeDb.Month.ShouldBe(month);
                startTimeDb.Day.ShouldBe(day);
                startTimeDb.Minute.ShouldBe(min);
                startTimeDb.Second.ShouldBe(sec);
                startTimeDb.Millisecond.ShouldBe(millisec);
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
                // endTimeDb.ShouldBe(endTime) does not work, so have to do it pretty heavy like this.
                int year = endTime.Year;
                int month = endTime.Month;
                int day = endTime.Day;
                int hour = endTime.Hour;
                int min = endTime.Minute;
                int sec = endTime.Second;
                int millisec = endTime.Millisecond;

                var timerDb = TimerModel.GetTimerById(timer.Id);
                var endTimeDb = (DateTime)timerDb.EndTime;

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
            int oldTimerId = 0;
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
                oldTimerId = timer.Id;
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Reset();
                timer.Start();
            });

            Then("a new timer should be created", () =>
            {
                timer.Id.ShouldNotBe(oldTimerId);
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
                timer.StartTime.ShouldBeInstanceOfType<DateTime>();
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
                }
                catch (InvalidOperationException) { }
            });

            Then("we should get an exception");
        }

        [TestMethod]
        public void Two_TimerModels_With_Same_Properties_Should_Equal_Each_Other()
        {
            // Define common properties...
            int timerId = 5;
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.Add(new TimeSpan(5, 3, 2)); // 5 hours, 3 minutes, 2 seconds

            // Create first instance...
            ITimeU.Models.Timer data1 = new ITimeU.Models.Timer();
            data1.TimerID = timerId;
            data1.StartTime = startTime;
            data1.EndTime = endTime;

            // Create new instance with same properties...
            ITimeU.Models.Timer data2 = new ITimeU.Models.Timer();
            data2.TimerID = timerId;
            data2.StartTime = startTime;
            data2.EndTime = endTime;

            TimerModel a = new TimerModel(data1);
            TimerModel b = new TimerModel(data2);
            a.ShouldBe(b);

            data2.StartTime = endTime;
            TimerModel c = new TimerModel(data2);
            a.ShouldNotBe(c);
        }

        [TestMethod]
        public void Two_TimerModels_With_Different_Properties_Should_Not_Equal_Each_Other()
        {
            // Define common properties...
            int timerId = 5;
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.Add(new TimeSpan(5, 3, 2)); // 5 hours, 3 minutes, 2 seconds

            // Create first instance...
            ITimeU.Models.Timer data1 = new ITimeU.Models.Timer();
            data1.TimerID = timerId;
            data1.StartTime = startTime;
            data1.EndTime = endTime;

            // Create new instance with same properties...
            ITimeU.Models.Timer data2 = new ITimeU.Models.Timer();
            data2.TimerID = timerId;
            data2.StartTime = startTime;
            // Don't set end time.

            TimerModel a = new TimerModel(data1);
            TimerModel c = new TimerModel(data2);
            a.ShouldNotBe(c);
        }

        [TestMethod]
        public void We_Should_Have_A_List_With_Races()
        {
            var races = new List<RaceModel>();
            Given("we have a timer", () => timer = new TimerModel());

            When("we want to select a race", () => races = RaceModel.GetRaces());

            Then("the racelist should contain at least one race", () => races.Count.ShouldNotBe(0));
        }

        //[TestMethod]
        //public void Editing_A_Timestamp_Should_Give_A_New_Timestamp()
        //{
        //    var runtimemodel = new RuntimeModel();
        //    var newRuntimemodel = new RuntimeModel();
        //    Given("we have a runtime", () =>
        //    {
        //        runtimemodel = RuntimeModel.Create(200);
        //        timer = new TimerModel();
        //        timer.Start();
        //        timer.Runtimes.Add(runtimemodel);
        //    });

        //    When("we want to change the runtime", () =>
        //    {
        //        timer.EditRuntime(runtimemodel, 400);
        //        newRuntimemodel = timer.Runtimes.First();
        //    });

        //    Then("the new runtime shouldn't be equal to the previous", () => newRuntimemodel.Runtime.ShouldNotBe(runtimemodel.Runtime));
        //}

        //[TestMethod]
        //public void Deleting_A_Timestamp_Should_Reduce_The_Timestamplist_With_1()
        //{
        //    var runtime = new RuntimeModel();
        //    runtime.Runtime = 900;
        //    var listcount = 0;
        //    Given("we have a runtimelist", () => {
        //        timer = new TimerModel();
        //        timer.Start();
        //        timer.Runtimes.Add(runtime);
        //        listcount = timer.Runtimes.Count;
        //    });

        //    When("we want to delete a runtime", () => {
        //        timer.DeleteRuntime(runtime);
        //    });

        //    Then("the runtime list should be rduced with 1", () =>
        //    {
        //        timer.Runtimes.Count.ShouldBe(listcount - 1);
        //    });
        //}

        [TestMethod]
        public void Deleting_A_Runtime_From_Dictionary_Should_Reduce_The_Dictionary_With_1()
        {
            var runtime = new RuntimeModel();
            var listcount = 0;
            Given("we have a runtimelist", () =>
            {
                timer = new TimerModel();
                timer.Start();
                runtime = timer.AddRuntime(300);
                listcount = timer.RuntimeDic.Count;
            });

            When("we want to delete a runtime", () =>
            {
                timer.DeleteRuntime(runtime);
            });

            Then("the runtime list should be rduced with 1", () =>
            {
                timer.RuntimeDic.Count.ShouldBe(listcount - 1);
            });
        }

        [TestMethod]
        public void Editing_A_Runtime_From_Dictionary_Should_Give_A_New_Runtime()
        {
            var runtimemodel = new RuntimeModel();
            var newValue = 0;
            Given("we have a runtime", () =>
            {
                timer = new TimerModel();
                timer.Start();
                runtimemodel = timer.AddRuntime(500);
            });

            When("we want to change the runtime", () =>
            {
                timer.EditRuntime(runtimemodel.Id, 400);
                newValue = Convert.ToInt32(timer.RuntimeDic.First().Value);
            });

            Then("the new runtime shouldn't be equal to the previous", () => newValue.ShouldNotBe(runtimemodel.Runtime));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

    }
}
