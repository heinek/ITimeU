﻿using System;
using System.IO;
using System.Net;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Controllers;
using System.Web.Mvc;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class RuntimeControllerTest : ScenarioClass
    {
        private EventModel eventModel;
        private RaceModel race;
        private TimerModel timer;

        [TestInitialize]
        public void TestSetup()
        {
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("TestRace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
            timer = new TimerModel();
            timer.SaveToDb();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            eventModel.Delete();
            race.Delete();
            timer.Delete();
        }

        [TestMethod]
        public void The_Controller_Should_Be_Able_To_Save_The_Runtime_To_Db()
        {
            RuntimeController runtimeCtrl = null;
            CheckpointModel checkpoint = null;

            int runtime = 360000000; // Runtime, in milliseconds. Equals 100 hours.
            ContentResult webResponse = null;

            Given("we have a timer and the time keeper wants to save a runtime", () =>
            {
                checkpoint = new CheckpointModel("TheRuntimeCheckpoint", timer, race);
                checkpoint = new CheckpointModel("TheRuntimeCheckpoint", timer, race);
                checkpoint.Sortorder = 1;
            });

            When("the time keeper saves the runtime", () =>
            {
                runtimeCtrl = new RuntimeController();
                webResponse = (ContentResult)runtimeCtrl.Save(runtime.ToString(), checkpoint.Id.ToString());
            });

            Then("the runtime should be saved in the database", () =>
            {
                int runtimeId = Int32.Parse(webResponse.Content);
                RuntimeModel runtimeStoredInDb = RuntimeModel.getById(runtimeId);
                runtimeStoredInDb.Runtime.ShouldBe(runtime);
            });
        }
    }
}
