using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Controllers;
using System.Web.Mvc;
using ITimeU.Models;


namespace ITimeU.Tests.Controllers
{
    [TestClass]
    public class CheckpointControllerTest: ScenarioClass
    {
        CheckpointController testController;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void It_Should_Possible_To_Extract_Race_List()
        {
            ViewResult result = null;
            Given("We have a Checkpoint controller", () =>
            {
                 testController = new CheckpointController();
            });

            When("We load checkpoint create page", () =>
            {
                result = (ViewResult)testController.Create();
            });

            Then("We should get a list of races", () =>
            {
                result.Model.ShouldBeInstanceOfType<List<RaceModel>>();
            });
        }
    }

}
