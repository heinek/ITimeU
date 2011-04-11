using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;


namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for ClubControllerTest
    /// </summary>

    [TestClass]
    public class ClubControllerTest : ScenarioClass
    {

        private ClubController controller;
        private ClubModel club;

        [TestInitialize]
        public void TestStartup()
        {
            club = new ClubModel("Testklubb");
            club.Save();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            club.DeleteFromDb();
        }

        [TestMethod]
        public void We_Should_Get_A_List_Of_Clubs_When_Going_To_Index()
        {
            ViewResult result = null;

            Given("we have a controller", () =>
            {
                controller = new ClubController();
            });

            When("we want to go to index page", () =>
            {
                result = (ViewResult)controller.Index();
            });

            Then("view should show a list of clubs", () =>
            {
                result.Model.ShouldBeInstanceOfType<List<ClubModel>>();
            });
        }

        [TestMethod]
        public void We_Should_Get_A_ClubModel_When_Editing()
        {
            ViewResult result = null;

            Given("we have a club and a controller", () =>
            {
                controller = new ClubController();
            });

            When("we want to edit a club", () =>
            {
                result = (ViewResult)controller.Edit(club.Id);
            });

            Then("we should get a club", () =>
            {
                result.Model.ShouldBeInstanceOfType<ClubModel>();
            });
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Update_A_Club()
        {
            ViewResult result = null;
            string newName = "NewClubname";

            Given("we have a club and a controller", () =>
            {
                controller = new ClubController();
            });

            When("we want to update the club", () =>
            {
                club.Name = newName;
                controller.Edit(club);
            });

            Then("the club's name should be updated", () =>
            {
                using (var context = new Entities())
                {
                    var clubDb = context.Clubs.Single(clb => clb.ClubID == club.Id);
                    clubDb.Name.ShouldBe(newName);
                }
            });
        }
    }
}
