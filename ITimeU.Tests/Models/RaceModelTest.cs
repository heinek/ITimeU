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
    public class RaceModelTest : ScenarioClass
    {
        private RaceModel newRace;
        private EventModel newEvent;
        private Race raceDB;
        private Entities ctxDBTest;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            newEvent.Delete();
            newRace.Delete();
        }

        [TestInitialize]
        public void TestSetup()
        {
            ctxDBTest = new Entities();
            newEvent = new EventModel("TestEvent", DateTime.Today);
            newEvent.Save();
            newRace = new RaceModel("RaceModelTestRace", new DateTime(2009, 2, 3));
            newRace.EventId = newEvent.EventId;
            newRace.Save();
            raceDB = new Race();
            raceDB.EventId = newEvent.EventId;
            raceDB.Name = "TestingRaceModel";
            raceDB.Distance = 200;
            raceDB.StartDate = DateTime.Parse("10/03/2020");
            ctxDBTest.Races.AddObject(raceDB);
            ctxDBTest.SaveChanges();
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Add_New_Race_In_Database()
        {
            int racesBefore = 0;
            int racesAfter = 0;
            RaceModel newTestRace = null;


            Given("We want to insert new Race in the database", () =>
                {
                    newTestRace = new RaceModel("test", DateTime.Today);
                });

            When("We insert a new Race in database", () =>
                {
                    racesBefore = ctxDBTest.Races.Count();
                    newTestRace.EventId = newEvent.EventId;
                    newTestRace.Save();
                });

            Then("The new Race should exist in database", () =>
                {
                    racesAfter = ctxDBTest.Races.Count();
                    racesAfter.ShouldBe(racesBefore + 1);
                });
        }

        [TestMethod]
        public void We_should_Be_Able_To_Update_Race_Name()
        {
            string insertedName = newRace.Name;
            string updatedName = insertedName;

            Given("We have inserted a new Race in database", () =>
                {
                });

            When("We update Race name", () =>
                {

                    newRace.UpdateRaceName("TestingRaceModelUpdated");
                    var ctxTest = new Entities();
                    updatedName = ctxTest.Races.Where(raceid => raceid.RaceID == newRace.RaceId).Single().Name;
                });

            Then("Race name must be update", () =>
                {
                    updatedName.ShouldNotBe(insertedName);
                });
        }

        [TestMethod]
        public void We_should_Be_Able_To_Update_Race_Distance()
        {
            var insertedDistance = raceDB.Distance;
            var updatedDistance = insertedDistance;

            Given("We have inserted a new Race in database", () =>
            {
            });

            When("We update Race distance", () =>
            {
                newRace.UpdateRaceDistance(300);
            });

            Then("Race distance must be update", () =>
            {
                var ctxTest = new Entities();
                updatedDistance = ctxTest.Races.Where(raceid => raceid.RaceID == newRace.RaceId).Single().Distance;
                updatedDistance.ShouldNotBe(insertedDistance);
            });
        }

        [TestMethod]
        public void We_should_Be_Able_To_Update_Race_Start_Date()
        {
            var insertedDate = raceDB.StartDate;
            var updatedDate = insertedDate;

            Given("We have inserted a new Race in database", () =>
            {
            });

            When("We update Race name", () =>
            {
                newRace.UpdateRaceDate(DateTime.Parse("10/05/2020"));
            });

            Then("Race name must be update", () =>
            {
                var ctxTest = new Entities();
                updatedDate = ctxTest.Races.Where(raceid => raceid.RaceID == newRace.RaceId).Single().StartDate;
                updatedDate.ShouldNotBe(insertedDate);
            });
        }

        [TestMethod]
        public void We_Should_Retrieve_Single_Race()
        {
            Race insertedRace = null;
            Given("We have a Race in database", () =>
                {
                });

            When("We retrieve a single race", () =>
                {
                    insertedRace = RaceModel.GetRace(raceDB.RaceID);
                });

            Then("It should have same inserted data", () =>
                {
                    insertedRace.Name.ShouldBe(raceDB.Name);
                    insertedRace.Distance.ShouldBe(raceDB.Distance);
                    insertedRace.StartDate.ShouldBe(raceDB.StartDate);
                });
        }

        [TestMethod]
        public void We_Should_Get_All_Athletes_Connected_To_A_Race()
        {
            AthleteModel athlete = null;
            int athletesConnectedToRace = 0;

            Given("we have a race with athletes", () =>
            {
                athlete = new AthleteModel("Testing", "Tester");
                athlete.SaveToDb();
                athlete.ConnectToRace(newRace.RaceId);
            });

            When("we want to get all the athletes connected to that race", () =>
                {
                    athletesConnectedToRace = newRace.GetAthletes().Count;
                });
            Then("the number of athletes should be 1", () =>
            {
                athletesConnectedToRace.ShouldBe(1);
            });
        }

        [TestMethod]
        public void We_Should_Get_All_Athletes_Not_Connected_To_A_Race()
        {
            int athletesNotConnectedToRace = 0;
            AthleteModel athlete = null;

            Given("we have a race with athletes", () =>
            {
            });

            When("we want to add a new athlete", () =>
            {
                athlete = new AthleteModel("Testing", "Tester");
                athlete.SaveToDb();
                athletesNotConnectedToRace = newRace.GetAthletesNotConnected().Count;
                athlete.ConnectToRace(newRace.RaceId);
            });

            Then("the number of athletes not connected to the race should be reduced by 1", () =>
            {
                newRace.GetAthletesNotConnected().Count.ShouldBe(athletesNotConnectedToRace - 1);
                athlete.Delete();
            });
        }

        [TestMethod]
        public void We_Should_Remove_Athlete_From_Race()
        {
            AthleteModel athlete = null;
            int athletesConnectedToRace = 0;
            Given("we have a race and athletes connected to the race", () =>
            {
                athlete = new AthleteModel("Test", "Tester");
                athlete.SaveToDb();
                athlete.ConnectToRace(newRace.RaceId);
                athletesConnectedToRace = newRace.GetAthletes().Count;
            });

            When("we want to remove an athlete from the race", () =>
            {
                athlete.RemoveFromRace(newRace.RaceId);
            });

            Then("the list of athletes should be reduced with 1", () =>
            {
                newRace.GetAthletes().Count.ShouldBe(athletesConnectedToRace - 1);
            });
        }

        private void CheckAndDeletelDuplicateRace(Race existingRace)
        {
            using (var ctxDel = new Entities())
            {
                if (ctxDel.Races.Any(race => (race.Name == existingRace.Name && race.Distance == existingRace.Distance && race.StartDate == existingRace.StartDate)))
                {
                    var DelRace = ctxDel.Races.Where(race => (race.Name == existingRace.Name && race.Distance == existingRace.Distance && race.StartDate == existingRace.StartDate)).Single();
                    ctxDel.DeleteObject(DelRace);
                    ctxDel.SaveChanges();
                }
            }
        }

        private void DeleteUpdated(string name, int distance, DateTime date)
        {
            using (var ctxDel = new Entities())
            {
                if (ctxDel.Races.Any(race => (race.Name == name && race.Distance == distance && race.StartDate == date)))
                {
                    var DelRace = ctxDel.Races.Where(race => (race.Name == name && race.Distance == distance && race.StartDate == date)).Single();
                    ctxDel.DeleteObject(DelRace);
                    ctxDel.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void It_Should_Possible_To_Get_A_List_Of_Races_From_The_Database()
        {
            List<RaceModel> races = null;

            Given("we want to get a list of races");

            When("we fetch all races from the database", () =>
            {
                races = RaceModel.GetRaces();
            });

            Then("we should get a list of races", () =>
            {
                races.ShouldBeInstanceOfType<List<RaceModel>>();
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_A_List_Of_Races()
        {
            List<RaceModel> races = null;

            Given("we want to get a list of races");

            When("we get the list of races", () =>
            {
                races = RaceModel.GetRaces();
            });

            Then("we should get a list of races", () =>
            {
                races.ShouldBeInstanceOfType<List<RaceModel>>();
            });
        }
        [TestMethod]
        public void We_Should_Be_Able_A_Race_With_Same_Name_In_The_Same_Event()
        {
            string exp = "";
            var raceDup = new RaceModel();
            string raceName = newRace.Name;
            Given("We want to create a race with the same name as a previous race in the same race", () =>
            {
                raceDup.Name = raceName;
                raceDup.EventId = newEvent.EventId;
            });

            When("we save the race", () =>
            {
                try
                {
                    raceDup.Save();
                }
                catch (Exception ex)
                {
                    exp = ex.Message;
                }
            });
            Then("we should get an exception", () =>
            {
                exp.ShouldBe("Det eksisterer allerede et løp med samme navn for dette stevnet");
            });
        }

    }
}
