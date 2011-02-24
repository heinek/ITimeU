using System;
using System.Linq;
using System.Threading;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

/*
    Given("", () =>
    {

    });

    When("", () =>
    {

    });

    Then("", () =>
    {

    });
*/

namespace ITimeU.Tests.Models
{

    [TestClass]
    public class TimerModelTest : ScenarioClass
    {
        private TimerModel timer = null;
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
        public void Timer_Id_Should_Be_Zero_Before_Timer_Has_Saved_Data()
        {
            When("we create a new timer", () =>
            {
                timer = new TimerModel();
            });

            Then("its ID should be zero", () => timer.Id.ShouldBe(0));
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
        public void Stopping_A_Timer_Twice_Should_Throw_Exception()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we start the timer twice", () =>
            {
                timer.Stop();
                try
                {
                    timer.Stop();
                    false.ShouldBeTrue(); // Fail test, we shouldn't get here.
                }
                catch (InvalidOperationException) { }
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

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
            });

            When("we start the timer", () =>
            {
                timer.Start();
                startTime = timer.StartTime;
            });

            Then("the start time should be saved to the database", () =>
            {
                var timerDb = TimerModel.GetTimerById(timer.Id);

                var startTimeDb = timerDb.StartTime;
                startTimeDb.ShouldBe(startTime);
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
                var timerDb = TimerModel.GetTimerById(timer.Id);

                var endTimeDb = (DateTime)timerDb.EndTime;
                endTimeDb.ShouldBe(endTime);
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
        public void MyTestMethod()
        {
            TimerModel timerDb = null;

            Given("we have a timer", () =>
            {
                timer = new TimerModel();
            });

            When("we start the timer, and retrieve it from the database", () =>
            {
                timer.Start();
            });

            Then("the timer retrieved from the database should be started", () =>
            {
                timerDb = TimerModel.GetTimerById(timer.Id);
                timerDb.IsStarted.ShouldBeTrue();
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
        public void Restarting_A_Stopped_Timer_Should_Reset_The_Start_Time()
        {
            DateTime? oldStartTime = null;

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
                oldStartTime = timer.StartTime;
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                Thread.Sleep(100); // Avoid that the restarting happens too fast for StartTime to change.
                timer.Start();
            });

            Then("the start time should be set, and be later/higher than the old start time", () =>
            {
                Assert.IsTrue(timer.StartTime > oldStartTime);

                TimerModel timerDb = TimerModel.GetTimerById(timer.Id);
                timerDb.StartTime.ShouldBe(timer.StartTime);
            });
        }

        [TestMethod]
        public void Restarting_A_Stopped_Timer_Should_Reset_The_End_Time()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Start();
            });

            Then("the end time should be reset/null", () =>
            {
                timer.EndTime.ShouldBe(null);

                TimerModel timerDb = TimerModel.GetTimerById(timer.Id);
                timerDb.EndTime.ShouldBe(timer.EndTime);
            });
        }

        [TestMethod]
        public void Timer_Should_Be_Started_When_Restarting_A_Stopped_Timer()
        {
            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Start();
            });

            Then("the end time should be null", () =>
            {
                timer.IsStarted.ShouldBe(true);
            });
        }

        [TestMethod]
        public void Restarting_A_Timer_Should_Update_The_Existing_Database_Row()
        {
            int? oldTimerId = null;

            Given("we have a started timer", () =>
            {
                timer = new TimerModel();
                timer.Start();
                oldTimerId = timer.Id;
            });

            When("we stop and restart the timer", () =>
            {
                timer.Stop();
                timer.Start();
            });

            Then("the end time should be null", () =>
            {
                timer.Id.ShouldBe(oldTimerId);
            });
        }

        [TestMethod]
        public void Two_TimerModels_With_Same_Properties_Should_Equal_Each_Other()
        {
            ITimeU.Models.Timer timer1 = null;
            ITimeU.Models.Timer timer2 = null;

            TimerModel timerModel1 = null;
            TimerModel timerModel2 = null;

            Given("we have some common properties of two timers", () =>
            {
                // Common properties...
                int timerId = 5;
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.Add(new TimeSpan(5, 3, 2)); // 5 hours, 3 minutes, 2 seconds

                timer1 = createTimer(timerId, startTime, endTime);
                timer2 = createTimer(timerId, startTime, endTime);
            });

            When("we create two timers with the same properties", () =>
            {
                timerModel1 = new TimerModel(timer1);
                timerModel2 = new TimerModel(timer2);
            });

            Then("the two timers should equal each other (though not same instance)", () =>
            {
                timerModel1.ShouldNotBeSameAs(timerModel2);
                timerModel1.ShouldBe(timerModel2);
            });

        }

        [TestMethod]
        public void Two_TimerModels_With_Different_Properties_Should_Not_Equal_Each_Other()
        {
            ITimeU.Models.Timer timer1 = null;
            ITimeU.Models.Timer timer2 = null;

            TimerModel timerModel1 = null;
            TimerModel timerModel2 = null;

            Given("we have some common properties of two timers", () =>
            {
                // Common properties...
                int timerId = 5;
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.Add(new TimeSpan(5, 3, 2)); // 5 hours, 3 minutes, 2 seconds

                timer1 = createTimer(timerId, startTime, endTime);
                timer2 = createTimer(timerId, endTime, endTime);
            });

            When("we create two timers with the different properties", () =>
            {
                timerModel1 = new TimerModel(timer1);
                timerModel2 = new TimerModel(timer2);
            });

            Then("the two timers should not equal each other", () =>
            {
                timerModel1.ShouldNotBe(timerModel2);
            });
        }

        private static ITimeU.Models.Timer createTimer(int timerId, DateTime startTime, DateTime endTime)
        {
            ITimeU.Models.Timer timer = new ITimeU.Models.Timer();

            timer.TimerID = timerId;
            timer.StartTime = startTime;
            timer.EndTime = endTime;

            return timer;
        }

        [TestMethod]
        public void A_DateTime_Value_In_Db_Should_Equal_The_Actual_DateTime_Value()
        {
            ITimeU.Models.Timer t = null;
            Entities context = null;

            Given("we have a timer database entry with a start time", () =>
            {
                t = new ITimeU.Models.Timer();
                t.StartTime = new DateTime(2010, 8, 5, 23, 45, 40, 799);
            });

            When("we save the timer to the database", () =>
            {
                context = new Entities();
                context.Timers.AddObject(t);
                context.SaveChanges();
            });

            Then("the start time should be the same in the timer and in the database", () =>
            {
                var tDb = context.Timers.Single(tmr => tmr.TimerID == t.TimerID);
                tDb.StartTime.ShouldBe(t.StartTime);
            });
        }

        [TestMethod]
        public void Setting_A_Timers_Start_Time_Should_Be_Rounded()
        {
            ITimeU.Models.Timer t = null;
            Given("we have a timer database entry with a start time set to 42.972 seconds", () =>
            {
                t = new ITimeU.Models.Timer();
                t.StartTime = new DateTime(2010, 8, 5, 23, 45, 42, 972);
            });

            When("we create a timer model based on the timer entry", () =>
            {
                timer = new TimerModel(t);
            });

            Then("the time should be rounded to 43 seconds and 0 milliseconds", () =>
            {
                timer.StartTime.Value.Second.ShouldBe(43);
                timer.StartTime.Value.Millisecond.ShouldBe(0);
            });
        }

        [TestMethod]
        public void Setting_A_Timers_Start_Time_Should_Be_Rounded_Also_When_Saving_To_Db()
        {
            ITimeU.Models.Timer t = null;

            Given("we have a timer database entry with a start time set to 42.972 seconds", () =>
            {
                t = new ITimeU.Models.Timer();
                t.StartTime = new DateTime(2010, 8, 5, 23, 45, 42, 972);
            });

            When("we save the timer entry to database and retrieve it back", () =>
            {
                var context = new Entities();
                context.Timers.AddObject(t);
                context.SaveChanges();
            });

            Then("the time should be rounded to 43 seconds and 0 milliseconds", () =>
            {
                TimerModel timer = TimerModel.GetTimerById(t.TimerID);

                timer.StartTime.Value.Second.ShouldBe(43);
                timer.StartTime.Value.Millisecond.ShouldBe(0);
            });
        }
        [TestMethod]
        public void Deleting_A_Runtime_From_Dictionary_Should_Reduce_The_Dictionary_With_1()
        {
            var runtime = new RuntimeModel();
            var listcount = 0;
            Given("we have a runtimelist", () =>
            {
                timer = new TimerModel();
                timer.Start();
                runtime = timer.AddRuntime(300, 1);
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
                runtimemodel = timer.AddRuntime(500, 1);
            });

            When("we want to change the runtime", () =>
            {
                timer.EditRuntime(runtimemodel.Id, 0, 1, 32, 20);
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
