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
        public void It_Should_Be_Possible_To_Save_An_Athlete_To_Database()
        {
            AthleteModel athlete = null;

            Given("we have an athlete", () =>
            {
                athlete = new AthleteModel("Arne", "Hansen");
            });

            When("we save it to the database", () =>
            {
                athlete.SaveToDb();
            });

            Then("it should be saved to the database", () =>
            {
                AthleteModel athleteDb = AthleteModel.GetById(athlete.Id);
                athleteDb.FirstName.ShouldBe("Arne");
                athleteDb.LastName.ShouldBe("Hansen");
            });
        }
    }
}
