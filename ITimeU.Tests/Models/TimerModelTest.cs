﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{

    [TestClass]
    public class TimerModelTest : ScenarioClass
    {
        private TimerModel timer;
        private RaceModel race;
        private EventModel eventModel;
        private CheckpointModel checkpoint;
        private CheckpointModel checkpoint2;
        private RuntimeModel runtime;

        [TestInitialize]
        public void TestSetup()
        {
            timer = new TimerModel();
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("SomeRace", new DateTime(2007, 10, 3));
            race.EventId = eventModel.EventId;
            race.Save();
            checkpoint = new CheckpointModel("Checkpoint1", timer, race, 1);
            checkpoint2 = new CheckpointModel("Checkpoint2", timer, race, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
        }

        [TestMethod]
        public void A_Timer_Exists()
        {
            Given("we want to create a timer");

            When("we instantiate the timer");

            Then("we have a timer", () => timer.ShouldNotBeNull()
            );
        }

        [TestMethod]
        public void Timer_Id_Should_Be_Zero_Before_Timer_Has_Saved_Data()
        {
            TimerModel newTimer = null;
            When("we create a new timer", () =>
            {
                newTimer = new TimerModel();
            });

            Then("its ID should be zero", () => newTimer.Id.ShouldBe(0));
        }

        [TestMethod]
        public void A_Started_Timer_Must_Have_A_Starttime()
        {
            Given("we have an instance of the timerclass");
            When("we we click the startbutton", () => timer.Start());
            Then("the timer should have a starttime", () => timer.StartTime.ShouldBeInstanceOfType<DateTime>());
        }

        [TestMethod]
        public void A_Started_Timer_Should_Always_Return_The_Same_StartTime()
        {
            DateTime? startTime = null;
            Given("we have a timer");

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
            Given("we have a timer");

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
            string exmessage = "";
            Given("we have a started timer", () =>
                {
                    timer.Start();
                });

            When("we stop the timer twice", () =>
            {
                timer.Stop();
                try
                {
                    timer.Stop();
                    false.ShouldBeTrue(); // Fail test, we shouldn't get here.
                }
                catch (InvalidOperationException ex)
                {
                    exmessage = ex.Message;
                }
            });

            Then("we should get an exception", () =>
                {
                    exmessage.ShouldBe("Cannot stop a stopped timer");
                });
        }

        [TestMethod]
        public void The_Start_Time_Should_Be_Null_Before_Timer_Is_Started()
        {
            DateTime? startTime = new DateTime();

            Given("we have a timer");
            When("we fetch the start time", () => startTime = timer.StartTime);
            Then("the start time should be null", () => startTime.ShouldBeNull());
        }

        [TestMethod]
        public void The_Timer_Should_Initially_Not_Be_Started()
        {
            Given("we are going to create a timer");
            When("we create the timer");
            Then("the timer should not be started", () => timer.IsStarted.ShouldBeFalse());
        }

        [TestMethod]
        public void The_Start_Time_Should_Be_Saved_To_The_Database()
        {
            DateTime? startTime = new DateTime();

            Given("we have a timer");

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

            Given("we have a timer");

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
            TimerModel timerDb = null;
            Given("we have a timer database entry with a start time set to 42.972 seconds", () =>
            {
                t = new ITimeU.Models.Timer();
                t.StartTime = new DateTime(2010, 8, 5, 23, 45, 42, 972);
            });

            When("we create a timer model based on the timer entry", () =>
            {
                timerDb = new TimerModel(t);
            });

            Then("the time should be rounded to 43 seconds and 0 milliseconds", () =>
            {
                timerDb.StartTime.Value.Second.ShouldBe(43);
                timerDb.StartTime.Value.Millisecond.ShouldBe(0);
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
                TimerModel timerDb = TimerModel.GetTimerById(t.TimerID);

                timerDb.StartTime.Value.Second.ShouldBe(43);
                timerDb.StartTime.Value.Millisecond.ShouldBe(0);
            });
        }

        [TestMethod]
        public void Deleting_A_Runtime_From_Dictionary_Should_Reduce_The_Dictionary_With_1()
        {
            var runtime = new RuntimeModel();
            var listcount = 0;
            var checkpointid = 0;
            Given("we have a runtimelist", () =>
            {
                timer.Start();
                checkpointid = timer.GetFirstCheckpointId();
                runtime = timer.AddRuntime(300, checkpointid);
                listcount = timer.CheckpointRuntimes[checkpointid].Count;
            });

            When("we want to delete a runtime", () =>
            {
                timer.DeleteRuntime(runtime);
            });

            Then("the runtime list should be rduced with 1", () =>
            {
                timer.CheckpointRuntimes[checkpointid].Count.ShouldBe(listcount - 1);
            });
        }

        [TestMethod]
        public void Editing_A_Runtime_From_Dictionary_Should_Give_A_New_Runtime()
        {
            var runtimemodel = new RuntimeModel();
            var newValue = 0;
            var checkpointid = 0;
            Given("we have a runtime", () =>
            {
                timer.Start();
                checkpointid = timer.GetFirstCheckpointId();
                runtimemodel = timer.AddRuntime(500, checkpointid);
            });

            When("we want to change the runtime", () =>
            {
                timer.EditRuntime(runtimemodel.Id, 0, 1, 32, 20);
                newValue = Convert.ToInt32(timer.CheckpointRuntimes[checkpointid].First().Value);
            });

            Then("the new runtime shouldn't be equal to the previous", () => newValue.ShouldNotBe(runtimemodel.Runtime));
        }

        [TestMethod]
        public void Changing_A_Checkpoint_Should_Give_A_New_CheckpointId()
        {
            var initialCheckpointId = 0;
            Given("we have started a timer", () =>
            {
                timer.Start();
                initialCheckpointId = timer.GetFirstCheckpointId();
            });
            When("we want to change the checkpoint", () =>
            {
                timer.ChangeCheckpoint(timer.GetCheckpoints()[1].CheckpointID);
            });
            Then("the checkpointid should be different", () =>
            {
                timer.CurrentCheckpointId.ShouldNotBe(initialCheckpointId);
            });
        }

        [TestMethod]
        public void Changing_A_Checkpoint_Should_Give_A_New_CheckpointList()
        {
            Dictionary<int, int> initialruntimes = new Dictionary<int, int>();
            Given("we have started a timer", () =>
            {
                timer.Start();
                initialruntimes = timer.CheckpointRuntimes[timer.GetFirstCheckpointId()];
            });
            When("we want to change the checkpoint", () =>
            {
                timer.ChangeCheckpoint(timer.GetCheckpoints()[1].CheckpointID);
                timer.AddRuntime(400, timer.GetCheckpoints()[1].CheckpointID);
            });
            Then("the checkpointlist should be different", () =>
            {
                timer.CheckpointRuntimes[timer.CurrentCheckpointId].ShouldNotBe(initialruntimes);
            });
        }

        [TestMethod]
        public void Adding_A_Runtime_To_A_Checkpoint_Then_Changing_To_New_Checkpoint_Then_Going_Back_To_First_Checkpoint_Should_Give_Same_Runtimelist_As_Before()
        {
            var initialruntimes = new Dictionary<int, int>();
            Given("we have started a timer and added a runtime", () =>
            {
                timer.Start();
                timer.AddRuntime(400, timer.GetFirstCheckpointId());
                initialruntimes = timer.CheckpointRuntimes[timer.CurrentCheckpointId];
            });
            When("we want to change the checkpoint and add a new runtime, and change back to first checkpoint", () =>
            {
                timer.ChangeCheckpoint(timer.GetCheckpoints()[1].CheckpointID);
                timer.AddRuntime(600, timer.GetCheckpoints()[1].CheckpointID);
                timer.ChangeCheckpoint(timer.GetFirstCheckpointId());
            });
            Then("the runtimelist should be the same", () =>
            {
                initialruntimes.ShouldBeSameAs(timer.CheckpointRuntimes[timer.GetFirstCheckpointId()]);
            });
        }

        [TestMethod]
        public void Adding_A_Runtime_To_A_Checkpoint_Then_Changing_To_New_Checkpoint_Then_Going_Back_To_First_Checkpoint_Then_Going_Back_To_Second_Checkpoint_Should_Give_Same_Runtimelist_As_Before()
        {
            var initialruntimes = new Dictionary<int, int>();
            Given("we have started a timer and added a runtime and then change", () =>
            {
                timer.Start();
                timer.AddRuntime(400, timer.GetFirstCheckpointId());
                timer.ChangeCheckpoint(timer.GetCheckpoints()[1].CheckpointID);
                timer.AddRuntime(600, timer.CurrentCheckpointId);
                initialruntimes = timer.CheckpointRuntimes[timer.CurrentCheckpointId];
            });
            When("we want to change the checkpoint and add a new runtime, and change back to first checkpoint, add a new runtime and then go back to second checkpoint", () =>
            {
                timer.ChangeCheckpoint(timer.GetFirstCheckpointId());
                timer.AddRuntime(800, timer.CurrentCheckpointId);
                timer.ChangeCheckpoint(timer.GetCheckpoints()[1].CheckpointID);
            });
            Then("the runtimelist should be the same", () =>
            {
                initialruntimes.ShouldBeSameAs(timer.CheckpointRuntimes[timer.GetCheckpoints()[1].CheckpointID]);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_The_Starttime_Of_A_Running_Timer()
        {
            DateTime? starttime = null;

            Given("we have started a timer", () =>
            {
                timer.Start();
            });

            When("we want to see the running timer, we should get the starttime of the timer", () =>
            {
                starttime = timer.StartTime;
            });

            Then("the starttime should not be null", () => starttime.ShouldNotBeNull());
        }


        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            checkpoint.Delete();
            timer.Delete();
            race.Delete();
            eventModel.Delete();
        }
    }
}
