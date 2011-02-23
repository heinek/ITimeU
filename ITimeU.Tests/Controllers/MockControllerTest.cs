using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using System.IO;
using ITimeU.Controllers;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class MockControllerTest : ScenarioClass
    {

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void Test_Mock_Session()
        {
            MockController controller = null;

            Given("we have a timer controller with a fake session", () =>
            {
                controller = new MockController();
                var sessionItems = new System.Web.SessionState.SessionStateItemCollection();
                controller.SetFakeControllerContext();
            });

            When("we create a test session", () => controller.TestSession());

            Then("the test session should contain the right content", () =>
            {
                controller.HttpContext.Session["item2"].ShouldBe("This is used for testing a mock session.");
            });
        }
    }
}
