using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class AthleteModelTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_A_List_Of_Athletes_To_Database()
        {
            List<AthleteModel> athletes = null;
            int? previousAthleteDbCount = AthleteModel.GetAll().Count;

            Given("we have a list of athletes", () =>
            {
                athletes = new List<AthleteModel>();
                athletes.Add(new AthleteModel("Arne", "Hansen"));
                athletes.Add(new AthleteModel("Geir", "Olsen"));
                athletes.Add(new AthleteModel("Per", "Iversen"));
            });

            When("we save the list to the database", () =>
            {
                AthleteModel.SaveToDb(athletes);
            });

            Then("the athletes in the list should be saved in the database", () =>
            {
                int athleteDbCount = AthleteModel.GetAll().Count;
                athleteDbCount.ShouldBe(previousAthleteDbCount + 3);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_An_Athlete_To_The_Database()
        {
            AthleteModel athlete = null;

            Given("we have an athlete", () =>
            {
                athlete = new AthleteModel("Arne", "Hansen");
            });

            When("we save the athlete to the database", () =>
            {
                athlete.SaveToDb();
            });

            Then("it should be saved to the database", () =>
            {
                AthleteModel athleteDb = AthleteModel.GetById(athlete.Id);
                athleteDb.Id.ShouldBe(athlete.Id);
                athleteDb.FirstName.ShouldBe(athlete.FirstName);
                athleteDb.LastName.ShouldBe(athlete.LastName);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_Athlete_Details_To_The_Database()
        {
            AthleteModel athlete = null;

            Given("we have an athlete", () =>
            {
                athlete = new AthleteModel("Per", "Olsen", 1997, ClubModel.GetOrCreate("Strindheim"), AthleteClassModel.GetOrCreate("G14"), 26);
            });

            When("we save the athlete to the database", () =>
            {
                athlete.SaveToDb();
            });

            Then("it should be saved to the database", () =>
            {
                AthleteModel athleteDb = AthleteModel.GetById(athlete.Id);
                athleteDb.Id.ShouldBe(athlete.Id);
                athleteDb.FirstName.ShouldBe(athlete.FirstName);
                athleteDb.LastName.ShouldBe(athlete.LastName);
                athleteDb.Birthday.ShouldBe(athlete.Birthday);
                athleteDb.Club.ShouldBe(athlete.Club);
                athleteDb.AthleteClass.ShouldBe(athlete.AthleteClass);
                athleteDb.StartNumber.ShouldBe(athlete.StartNumber);
            });
        }


        [TestMethod]
        public void ToString_Should_Return_Firstname_And_Lastname()
        {
            string toString = null; 
            AthleteModel athlete = null;

            Given("we have an athlete", () =>
            {
                athlete = new AthleteModel("Arne", "Hansen");
            });

            When("we call ToString", () =>
            {
                toString = athlete.ToString();
            });

            Then("we should get firstname and lastname", () =>
            {
                toString.ShouldBe("Arne Hansen");
            });
        }

        [TestMethod]
        public void Attempting_To_Retrieve_An_Athlete_That_Does_Not_Exist_In_The_Db_Should_Give_An_Error()
        {
            AthleteModel athlete = null;
            int athleteIdThatDoesNotExist = -1;

            Given("we have a primary key for an athlete that does not exists in the database", () => {
                athlete = new AthleteModel("Per", "Olsen");
                athlete.SaveToDb();
                athleteIdThatDoesNotExist = athlete.Id;
                athlete.DeleteFromDb();
            });

            When("we attempt to fetch an athlete that does not exist in the database", () =>
            {
                try {
                    AthleteModel.GetById(athleteIdThatDoesNotExist);
                    false.ShouldBe(true); // Fail test if exception is not thrown.
                } catch (ModelNotFoundException e) {
                    Assert.IsTrue(e.Message.Length > 0);
                }
            });

            Then("an exception should have been thrown");
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Retrieve_A_List_Of_Athletes_From_DB()
        {
            var athletes = new List<AthleteModel>();
            Given("athletes is saved to the database");

            When("we want to retrieve alle the athletes from database", () =>
            {
                athletes = AthleteModel.GetAll();
            });

            Then("the list should not be null", () => athletes.ShouldNotBeNull());
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Connect_An_Athlete_To_A_Race()
        {
            RaceModel race = null;
            AthleteModel athlete = null;
            Given("we have a race and a athlete", () => {
                 race = new RaceModel("testrace", DateTime.Today);
                 race.Save();
                 athlete = new AthleteModel("Test", "Testesen");
                 athlete.SaveToDb();
            });

            When("we want to connect a athlete to that race", () =>
            {
                athlete.ConnectToRace(race.RaceId);
            });

            Then("the list of athletes for the race should be 1", () =>
            {
                race.GetAthletes().Count().ShouldBe(1);
            });
        }

        //[TestMethod]
        //public void It_Should_Be_Possible_To_Retrieve_A_List_Of_Athletes_Connected_To_A_Race()
        //{
        //    var athletes = new List<AthleteModel>();
        //    Given("we have a race athletes is connected to a race");

        //    When("we want to retrieve alle the athletes connected to a race", () =>
        //    {
        //        athletes = AthleteModel.GetAthletesForRace();
        //    });

        //    Then("the list should not be null", () => athletes.ShouldNotBeNull());
        //}
    }
}
