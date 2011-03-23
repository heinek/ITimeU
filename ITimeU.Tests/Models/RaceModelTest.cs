using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class RaceModelTest: ScenarioClass
    {
        [TestInitialize]
        public void TestSetup()
        { 
            
        }

        [TestMethod]
        public void We_Should_Be_Able_To_Add_New_Race_In_Database()
        {
            //Given("We have "
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
    }
}
