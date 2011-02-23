using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;
using ITimeU.Logging;

/// TODO:

namespace ITimeU.Tests.Models
{

    [TestClass]
    public class RuntimetModelTest : ScenarioClass
    {
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void MyTestMethod()
        {
            
        }

    }
}
