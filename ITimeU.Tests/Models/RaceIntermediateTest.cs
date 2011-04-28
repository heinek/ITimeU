using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

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
        private CheckpointOrderModel checkpointOrder;
        private RaceIntermediateModel intermediate;
        private CheckpointModel checkpoint;


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
            checkpointOrder = null;
            intermediate = null;
            checkpoint = null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            athlete.Delete();
            race.Delete();
            timer.Delete();
            club.DeleteFromDb();
            if (intermediate != null) intermediate.Delete();
            if (checkpointOrder != null) checkpointOrder.DeleteCheckpointOrderDB();
            if (checkpoint != null) checkpoint.Delete();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Connect_An_Athlete_To_A_Startnumber()
        {

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
            timer = new TimerModel();
            timer.RaceID = race.RaceId;
            checkpoint = new CheckpointModel("Checkpoint1", timer, race, 1);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }
    }
}
