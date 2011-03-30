using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class RaceModelTest: ScenarioClass
    {
        private RaceModel newRace;
        private Race raceDB;
        private Entities ctxDBTest;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestInitialize]
        public void TestSetup()
        {            
            ctxDBTest = new Entities();
            newRace = new RaceModel("RaceModelTestRace", new DateTime(2009, 2, 3));
            raceDB = new Race();

            raceDB.Name = "TestingRaceModel";
            raceDB.Distance = 200;
            raceDB.StartDate = DateTime.Parse("10/03/2020");

        }

        [TestMethod]
        public void We_Should_Be_Able_To_Add_New_Race_In_Database()
        {                         
            int racesBefore = 0;
            int racesAfter = 0;
            Given("We want to insert new Race in the database");

            When("We insert a new Race in database", () =>
                {
                    racesBefore = ctxDBTest.Races.Count();
                    CheckAndDeletelDuplicateRace(raceDB);
                    newRace = new RaceModel(raceDB);
                    newRace.Save();                   
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
            string insertedName = raceDB.Name;
            string updatedName = insertedName;
            var id = 0;
            
            
            Given("We have inserted a new Race in database", () =>
                {
                    CheckAndDeletelDuplicateRace(raceDB);
                    newRace = new RaceModel(raceDB);
                    newRace.Save();   
                    var ctxDB = new Entities();
                    id = ctxDB.Races.Where(race => (race.Name == raceDB.Name && race.Distance == raceDB.Distance && race.StartDate == raceDB.StartDate)).Single().RaceID;
                });

            When("We update Race name", () =>
                {
                    
                    newRace.UpdateRaceName(id,"TestingRaceModelUpdated");
                    var ctxTest = new Entities();
                    updatedName = ctxTest.Races.Where(raceid => raceid.RaceID == id).Single().Name;
                    
                });

            Then("Race name must be update", () =>
                {                    
                    updatedName.ShouldNotBe(insertedName);
                    DeleteUpdated(updatedName, (int)raceDB.Distance, raceDB.StartDate);
                });
        }

        [TestMethod]
        public void We_should_Be_Able_To_Update_Race_Distance()
        {           
            var insertedDistance = raceDB.Distance;
            var updatedDistance = insertedDistance;
            var id = 0;
            

            Given("We have inserted a new Race in database", () =>
            {
                CheckAndDeletelDuplicateRace(raceDB);
                newRace = new RaceModel(raceDB);
                newRace.Save();                
                var ctxDB = new Entities();
                id = ctxDB.Races.Where(race => (race.Name == raceDB.Name && race.Distance == raceDB.Distance && race.StartDate == raceDB.StartDate)).Single().RaceID;
            });

            When("We update Race distance", () =>
            {
                newRace.UpdateRaceDistance(id, 300);
            });

            Then("Race distance must be update", () =>
            {
                var ctxTest = new Entities();
                updatedDistance = ctxTest.Races.Where(raceid => raceid.RaceID == id).Single().Distance;
                updatedDistance.ShouldNotBe(insertedDistance);
                DeleteUpdated(raceDB.Name, (int)updatedDistance, raceDB.StartDate);
            });
        }

        [TestMethod]
        public void We_should_Be_Able_To_Update_Race_Start_Date()
        {            
            var insertedDate = raceDB.StartDate;
            var updatedDate = insertedDate;
            var id = 0;            

            Given("We have inserted a new Race in database", () =>
            {
                CheckAndDeletelDuplicateRace(raceDB);
                newRace = new RaceModel(raceDB);
                newRace.Save();   
                var ctxDB = new Entities();
                id = ctxDB.Races.Where(race => (race.Name == raceDB.Name && race.Distance == raceDB.Distance && race.StartDate == raceDB.StartDate)).Single().RaceID;
            });

            When("We update Race name", () =>
            {
                newRace.UpdateRaceDate(id, DateTime.Parse("10/05/2020"));
            });

            Then("Race name must be update", () =>
            {
                var ctxTest = new Entities();
                updatedDate = ctxTest.Races.Where(raceid => raceid.RaceID == id).Single().StartDate;
                updatedDate.ShouldNotBe(insertedDate);
                DeleteUpdated(raceDB.Name, (int)raceDB.Distance, updatedDate);
            });
        }

        [TestMethod]
        public void We_Should_Retrieve_Single_Race()
        {
            int id = 0;
            Race insertedRace = new Race();
            Given ("We have a Race in database", () =>
                {
                    CheckAndDeletelDuplicateRace(raceDB);
                    newRace = new RaceModel(raceDB);
                    newRace.Save();   
                    var ctxDB = new Entities();
                    id = ctxDB.Races.Where(race => (race.Name == raceDB.Name && race.Distance == raceDB.Distance && race.StartDate == raceDB.StartDate)).Single().RaceID;                    
                });

            When ("We retrieve a single race", () =>
                {
                    insertedRace = newRace.GetRace(id);
                });

            Then("It should have same inserted data", () =>
                {
                    insertedRace.Name.ShouldBe(raceDB.Name);
                    insertedRace.Distance.ShouldBe(raceDB.Distance);
                    insertedRace.StartDate.ShouldBe(raceDB.StartDate);
                    CheckAndDeletelDuplicateRace(raceDB);
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

    }
}
