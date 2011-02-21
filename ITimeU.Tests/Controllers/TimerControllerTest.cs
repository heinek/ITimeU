using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;

namespace ITimeU.Tests.Controllers
{
    /// <summary>
    /// Summary description for TimerControllerTest
    /// </summary>
    [TestClass]
    public class TimerControllerTest : ScenarioClass
    {
        private TimerModel timer = null;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

    }
}
