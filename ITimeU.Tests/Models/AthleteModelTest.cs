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
    }
}
