using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for AthleteControllerTest
    /// </summary>
    [TestClass]
    public class AthleteControllerTest : ScenarioClass
    {
        private AthleteController controller;

        [TestInitialize]
        public void TestStartup()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void We_Should_Get_A_List_Of_Athletes_Connected_To_A_Race()
        {
            ViewResult result = null;
            RaceModel race = null;
            AthleteModel athlete = null;

            Given("we have a race connected to an athlete and a controller", () =>
            {
                controller = new AthleteController();
                var eventModel = new EventModel("Test", DateTime.Today);
                eventModel.Save();
                race = new RaceModel("testrace", DateTime.Today);
                race.EventId = eventModel.EventId;
                race.Save();
                athlete = new AthleteModel("Test", "Testesen");
                athlete.ConnectToRace(race.RaceId);
                athlete.SaveToDb();
            });

            When("we want to see athletes for a race", () =>
            {
                result = (ViewResult)controller.AthletesForRace(race.RaceId);
            });

            Then("view should show a list of athletes", () =>
            {
                result.Model.ShouldBeInstanceOfType<List<AthleteModel>>();
            });
        }
    }
}
