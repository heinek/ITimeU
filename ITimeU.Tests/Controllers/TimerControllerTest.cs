using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for TimerControllerTest
    /// </summary>
    [TestClass]
    public class TimerControllerTest : ScenarioClass
    {

        private TimerModel timer;
        private RaceModel race;
        private EventModel eventModel;
        private CheckpointModel checkpoint;

        [TestInitialize]
        public void TestSetup()
        {
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("TestRace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
            timer = new TimerModel();
            timer.RaceID = race.RaceId;
            timer.SaveToDb();
            checkpoint = new CheckpointModel("Hemsedal", timer, race, 1);
            checkpoint.SaveToDb();
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            timer.Delete();
            checkpoint.Delete();
            race.Delete();
            eventModel.Delete();
        }

        [TestMethod]
        public void A_Checkpoint_Must_Have_A_Timer_When_User_Starts_Timer()
        {
            TimerController timerCtrl = null;

            Given("the user has selected a checkpoint", () =>
            {
                race.Save();
                checkpoint = new CheckpointModel("Hemsedal", timerInView, race, 1);
                timerCtrl = new TimerController();
                setMockSessionFor(timerCtrl);
            });

            When("the user selects a checkpoint and clicks OK", () =>
            {
                ViewResult ctrlResult = (ViewResult)timerCtrl.Index(race.RaceId);
                timer = (TimerModel)ctrlResult.Model;
            });

            Then("the checkpoint's timer should be associated with the timer in the view", () =>
            {
                // We currently have to re-fetch the checkpoint from database, because the TimerController
                // updates its own instance of the CheckpointModel, not the checkpoint instance we're using
                // here.
                CheckpointModel checkpointDb = CheckpointModel.getById(checkpoint.Id);
                timer.ShouldBe(checkpointDb.Timer);
            });
        }

            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, race, 1);
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, race, 2);
        private static void setMockSessionFor(TimerController timerCtrl)
        {
            var sessionItems = new System.Web.SessionState.SessionStateItemCollection();
            timerCtrl.SetFakeControllerContext();
        }

    }
}
