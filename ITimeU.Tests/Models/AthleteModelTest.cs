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
        public void Two_ClubModels_With_Same_Properties_Should_Equal_Each_Other()
        {
            string name = null;
            ClubModel clubModel1 = null;
            ClubModel clubModel2 = null;

            Given("we have some common properties of two clubs", () =>
            {
                // Common properties...
                name = "Trondheim";

            });

            When("we create two clubs with the same properties", () =>
            {
                clubModel1 = ClubModel.GetOrCreate(name);
                clubModel2 = ClubModel.GetOrCreate(name);
            });

            Then("the two clubs should equal each other (though not same instance)", () =>
            {
                clubModel1.ShouldBe(clubModel2);
            });

        }

        [TestMethod]
        public void Two_ClubModels_With_Different_Properties_Should_Not_Equal_Each_Other()
        {
            ClubModel clubModel1 = null;
            ClubModel clubModel2 = null;

            Given("we want to create two clubs with different names"); 

            When("we create two clubs with different properties", () =>
            {
                clubModel1 = ClubModel.GetOrCreate("Lade");
                clubModel2 = ClubModel.GetOrCreate("Malvik");
            });

            Then("the two clubs should not equal each other", () =>
            {
                clubModel1.ShouldNotBe(clubModel2);
            });
        }

    }
}
