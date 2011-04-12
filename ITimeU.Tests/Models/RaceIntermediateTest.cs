using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class RaceIntermediateTest : ScenarioClass
    {
        private AthleteModel athlete;
        private EventModel eventModel;
        private RaceModel race;
        private ClubModel club;
        private TimerModel timer;

        [TestInitialize]
        public void TestSetup()
        {
            club = new ClubModel("Test IK");
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            athlete = new AthleteModel("Tester", "Test");
            athlete.StartNumber = 1;
            athlete.Club = club;
            athlete.SaveToDb();
            race = new RaceModel("TestRace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            athlete.Delete();
            race.Delete();
            timer.Delete();
            club.DeleteFromDb();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Connect_An_Athlete_To_A_Startnumber()
        {
            CheckpointOrderModel checkpointOrder;
            RaceIntermediateModel intermediate;

            Given("we have an athlete and a startnumber registrert", () =>
            {

                athlete.ConnectToRace(race.RaceId);
                timer = CreateNewTimerModelWithCheckpoints(race);
                checkpointOrder = new CheckpointOrderModel();
                checkpointOrder.AddCheckpointOrderDB(timer.CurrentCheckpointId, 1);
                timer.Start();
                timer.AddRuntime(400, timer.GetFirstCheckpointId());
                intermediate = new RaceIntermediateModel(timer.CurrentCheckpointId, checkpointOrder.ID, timer.CheckpointRuntimes[timer.CurrentCheckpointId].First().Key);
                intermediate.Save();
            });

            When("we want to connect athletes to startnumbers", () =>
            {
                RaceIntermediateModel.MergeAthletes(race.RaceId);
            });

            Then("athletes should be connected to raceintermediate", () =>
            {
                RaceIntermediateModel.GetRaceintermediatesForRace(race.RaceId).First().AthleteId.ShouldNotBeNull();

            });

        }
        /// <summary>
        /// Creates the new timer model with checkpoints.
        /// </summary>
        /// <returns></returns>
        private TimerModel CreateNewTimerModelWithCheckpoints(RaceModel race)
        {
            var timer = new TimerModel();
            timer.RaceID = race.RaceId;
            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, race, 1);
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, race, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }
    }
}
