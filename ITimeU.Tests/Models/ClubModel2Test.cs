using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class ClubModel2Test : ScenarioClass
    {

        private const string CLUB_BYAASEN = "Byåsen";

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void TestGetAll()
        {
            List<ClubModel2> clubs = ClubModel2.GetAll();
            Assert.IsTrue(clubs.Count >= 0); 
        }

    }
}
