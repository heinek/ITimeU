using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

/// TODO:

namespace ITimeU.Tests.Models
{

    [TestClass]
    public class RuntimetModelTest : ScenarioClass
    {
        private TimerModel timer;
        private RaceModel race;
        private EventModel eventModel;
        private CheckpointModel checkpoint01;
        private CheckpointModel checkpoint02;

        [TestInitialize]
        public void TestSetup()
        {
            eventModel = new EventModel("Testevent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("TestRace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
            timer = new TimerModel();
            timer.RaceID = race.RaceId;
            timer.SaveToDb();
            checkpoint01 = new CheckpointModel("Checkpoint1", timer, race, 1);
            checkpoint02 = new CheckpointModel("Checkpoint2", timer, race, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            timer.Delete();
            race.Delete();
            eventModel.Delete();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_The_Runtimes_For_A_Race()
        {
            Given("we have a timer");

            When("we add runtimes", () => timer.AddRuntime(400, timer.GetFirstCheckpointId()));

            Then("we should be able to get all the runtimes", () =>
            {
                var runtimes = RuntimeModel.GetRuntimes(timer.GetFirstCheckpointId());
                runtimes.ShouldNotBeNull();
            });
        }

        [TestMethod]
        public void The_List_Of_Runtimes_Sould_Return_A_Runtime_After_It_Has_Been_Saved_To_Db()
        {
            Given("we have a timer");

            When("we add runtimes", () => timer.AddRuntime(400, timer.GetFirstCheckpointId()));

            Then("we should be able to get back the runtime we added", () =>
            {
                var runtimes = RuntimeModel.GetRuntimes(timer.GetFirstCheckpointId());
                runtimes.First().Value.ShouldBe(400);
            });
        }
    }
}
