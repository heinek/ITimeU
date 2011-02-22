using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Library;

namespace ITimeU.Tests.Library
{
    [TestClass]
    public class DateTimeRounderTest : ScenarioClass
    {

        [TestMethod]
        public void DateTimeRounder_Should_Round_Milliseconds_To_Nearest_Hundreds()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 250);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Millisecond.ShouldBe(300);
        }

        [TestMethod]
        public void DateTimeRounder_Should_Round_Zero_Milliseconds_To_Zero()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 0);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Millisecond.ShouldBe(0);
        }
        
        [TestMethod]
        public void DateTimeRounder_Should_Round_949_Milliseconds_To_900()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 949);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Millisecond.ShouldBe(900);
        }

        [TestMethod]
        public void DateTimeRounder_Should_Round_950_Milliseconds_To_Zero()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 950);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Millisecond.ShouldBe(0);
        }

        [TestMethod]
        public void DateTimeRounder_Should_Round_972_Milliseconds_To_Zero()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 972);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Millisecond.ShouldBe(0);
        }

        [TestMethod]
        public void DateTimeRounder_Should_Round_950_Milliseconds_To_Zero_And_Increase_Second()
        {
            DateTime original = new DateTime(2010, 6, 5, 10, 32, 42, 950);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Second.ShouldBe(43);
        }

        [TestMethod]
        public void DateTimeRounder_Should_Round_950_Milliseconds_To_Zero_And_Increase_Everything_After()
        {
            DateTime original = new DateTime(2010, 12, 31, 23, 59, 59, 950);
            DateTime rounded = DateTimeRounder.RoundToOneDecimal(original);
            rounded.Year.ShouldBe(2011);
        }
    }
}
