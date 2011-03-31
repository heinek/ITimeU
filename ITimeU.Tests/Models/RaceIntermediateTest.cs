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
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Connect_An_Athlete_To_A_Startnumber()
        {
            AthleteModel athlete = null;
            RaceModel race = null;
            TimerModel timer = null;
            ClubModel club = null;
            CheckpointOrderModel checkpointOrder;
            RaceIntermediateModel intermediate;

            Given("we have an athlete and a startnumber registrert", () =>
            {
                club = new ClubModel("Test IK");
                athlete = new AthleteModel("Tester", "Test");
                athlete.StartNumber = 1;
                athlete.Club = club;
                athlete.SaveToDb();
                race = new RaceModel("TestRace", DateTime.Today);
                race.Save();
                athlete.ConnectToRace(race.RaceId);
                timer = CreateNewTimerModelWithCheckpoints(race.RaceId);
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
        private TimerModel CreateNewTimerModelWithCheckpoints(int raceid)
        {
            var timer = new TimerModel();
            timer.RaceID = raceid;
            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            checkpoint1.RaceId = raceid;
            checkpoint1.Update();
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            checkpoint2.RaceId = raceid;
            checkpoint2.Update();
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }
    }
}
