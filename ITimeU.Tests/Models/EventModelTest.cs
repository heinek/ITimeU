using System;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for EventModelTest
    /// </summary>
    [TestClass]
    public class EventModelTest : ScenarioClass
    {

        private EventModel newEvent;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            //newEvent.Delete();
        }

        [TestInitialize]
        public void TestSetup()
        {
        }

        [TestMethod]
        public void Creating_An_Event_With_An_Invalid_Date_Should_Throw_An_Exception()
        {
            string exmessage = "";
            string expectedmessage = "Ugyldig dato for stevne";
            Given("we want to create a new event");

            When("we create the event", () =>
            {
                newEvent = new EventModel("Testevent", new DateTime(2000, 1, 1));
                try
                {
                    newEvent.Save();
                }
                catch (ArgumentException ex)
                {
                    exmessage = ex.Message;
                }
            });

            Then("the exceptionmessage should be: " + expectedmessage, () =>
            {
                exmessage.ShouldBeSameAs(expectedmessage);
            });
        }
    }
}
