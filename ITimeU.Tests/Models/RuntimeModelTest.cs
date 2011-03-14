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
        public void It_Should_Be_Possible_To_Get_The_Runtimes_For_A_Race()
        {
            var timer = new TimerModel();
            Given("we have a timer", () => timer = CreateNewTimerModelWithCheckpoints());

            When("we add runtimes", ()=> timer.AddRuntime(400, timer.GetFirstCheckpointId()));

            Then("we should be able to get all the runtimes", ()=>
            {
                var runtimes = RuntimeModel.GetRuntimes(timer.GetFirstCheckpointId());
                runtimes.ShouldNotBeNull();
            });
        }

        [TestMethod]
        public void The_List_Of_Runtimes_Sould_Return_A_Runtime_After_It_Has_Been_Saved_To_Db()
        {
            var timer = new TimerModel();
            Given("we have a timer", () => timer = CreateNewTimerModelWithCheckpoints());

            When("we add runtimes", () => timer.AddRuntime(400, timer.GetFirstCheckpointId()));

            Then("we should be able to get back the runtime we added", () =>
            {
                var runtimes = RuntimeModel.GetRuntimes(timer.GetFirstCheckpointId());
                runtimes.First().Value.ShouldBe(400);
            });
        }
        /// <summary>
        /// Creates the new timer model with checkpoints.
        /// </summary>
        /// <returns></returns>
        private TimerModel CreateNewTimerModelWithCheckpoints()
        {
            var timer = new TimerModel();
            var checkpoint1 = new CheckpointModel("Checkpoint1", timer, 1);
            var checkpoint2 = new CheckpointModel("Checkpoint2", timer, 2);
            timer.CurrentCheckpointId = timer.GetFirstCheckpointId();
            timer.CheckpointRuntimes.Add(timer.CurrentCheckpointId, new Dictionary<int, int>());
            return timer;
        }

    }
}
