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
        private EventModel eventModel;
        private RaceModel race;
        private AthleteModel athlete;

        [TestInitialize]
        public void TestStartup()
        {
            eventModel = new EventModel("Test", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("testrace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
            athlete = new AthleteModel("Test", "Testesen");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            eventModel.Delete();
            race.Delete();
            athlete.Delete();
        }

        [TestMethod]
        public void We_Should_Get_A_List_Of_Athletes_Connected_To_A_Race()
        {
            ViewResult result = null;

            Given("we have a race connected to an athlete and a controller", () =>
            {
                controller = new AthleteController();
                athlete.SaveToDb();
                athlete.ConnectToRace(race.RaceId);
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
