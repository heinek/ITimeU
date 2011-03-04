using System;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class ImportAthletesTest : ScenarioClass
    {
        [TestMethod]
        public void We_Should_Have_A_List_Of_Imported_Athletes_From_FriRes()
        {
            
            Given("we have a athlete model", () => athletes = new AthleteModel()
            );
            When("we imports athletes from FriRes", () => athletes = AthleteModel.getAthletes()
            );
            Then("we should have a list of athletes", () => athletes.ShouldNoteNull()
            );

        }

        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }
    
    }

}
