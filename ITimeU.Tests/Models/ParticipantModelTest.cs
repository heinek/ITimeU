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
    /// <summary>
    /// Summary description for ParticipantModelTest
    /// </summary>
    [TestClass]
    public class ParticipantModelTest : ScenarioClass
    {

        private ParticipantModel participantModel = null;

        [TestMethod]
        public void Function_Name()
        {
            Scenario("We have a participant model");
            Given("we want to create a participant model");

            When("we instantiate the participant model class",
                () => participantModel = new ParticipantModel()
            );

            Then("we have a participant model", () => participantModel.ShouldNotBeNull()
            );
        }

        [TestMethod]
        public void Test_Name()
        {
            Given("");

            When("");

            Then("");
        }


        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
