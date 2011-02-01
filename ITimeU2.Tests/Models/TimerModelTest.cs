using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU2.Models;

namespace ITimeU2.Tests.Models
{
    /// <summary>
    /// Summary description for TimerModelTest
    /// </summary>
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {

        private static TimerModel timerModel = null;

        [TestMethod]
        public void We_Have_A_Timer_Model()
        {
            Scenario("We have a timer model");
            Given("we want to create a timer model");

            When("we instantiate the timer model class", 
                () => timerModel = new TimerModel()
            );

            Then("we have a timer model", () => 
                Assert.IsNotNull(timerModel)
            );
        }

        [TestMethod]
        public void The_TimerModel_Has_A_Starttime()
        {
            Given("we have a instance of the timerclass", () => timerModel = new TimerModel());

            When("we we click the startbutton", () => timerModel.Start());

            Then("the timer should have a starttime", () => Assert.IsNotNull(timerModel.GetStarttime()));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
 
       
    }
}
