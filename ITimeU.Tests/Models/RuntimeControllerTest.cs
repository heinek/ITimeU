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

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class RuntimeControllerTest : ScenarioClass
    {

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        /// <summary>
        /// NOTE: The web server must be started in order for this test to run correctly.
        /// </summary>
        [TestMethod]
        public void The_Runtime_Should_Be_Saved_To_Database_On_Http_Request()
        {
            HttpWebRequest httpRequest = null;
            int runtime = 3424; // just a random number
            String requestUrl = null;
            RuntimeModel runtimeStoredInDb = null;

            Given("the time keeper wants to save a runtime", () =>
            {
                requestUrl = @"http://localhost:54197/Runtime/Save/?runtime=" + runtime;
            });

            When("the time keeper saves the runtime", () =>
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                String httpResponse = doHttpRequest(httpRequest);
                int runtimeId = Int32.Parse(httpResponse);
                runtimeStoredInDb = RuntimeModel.getById(runtimeId);
            });

            Then("the runtime should be saved in the database", () =>
            {
                runtimeStoredInDb.Runtime.ShouldBe(runtime);
            });
        }

        private string doHttpRequest(HttpWebRequest httpRequest)
        {
            // Get and read HTTP respose...
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            StreamReader responseStream = new StreamReader(httpResponse.GetResponseStream());
            string response = responseStream.ReadToEnd();
            responseStream.Close();
            httpResponse.Close();

            return response;
        }


    }
}
