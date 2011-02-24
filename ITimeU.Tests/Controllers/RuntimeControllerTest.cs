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
using ITimeU.Models;

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
            int runtime = 360000000; // Runtime, in milliseconds. Equals 100 hours.
            String requestUrl = null;
            String webResponse = null;

            Given("the time keeper wants to save a runtime", () =>
            {
                CheckpointModel checkpoint = new CheckpointModel("TheRuntimeCheckpoint");
                requestUrl = @"http://localhost:54197/Runtime/Save/" +
                    "?runtime=" + runtime +
                    "&checkpointId=" + checkpoint.Id;
            });

            When("the time keeper saves the runtime", () =>
            {
                webResponse = visit(requestUrl); ;
            });

            Then("the runtime should be saved in the database", () =>
            {
                int runtimeId = Int32.Parse(webResponse);
                RuntimeModel runtimeStoredInDb = RuntimeModel.getById(runtimeId);
                runtimeStoredInDb.Runtime.ShouldBe(runtime);
            });
        }

        private string visit(string requestUrl)
        {
            HttpWebResponse httpResponse = doHttpRequest(requestUrl);
            string response = readHttpResponse(httpResponse);
            httpResponse.Close();

            return response;
        }

        private static HttpWebResponse doHttpRequest(string requestUrl)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            return httpResponse;
        }

        private static string readHttpResponse(HttpWebResponse httpResponse)
        {
            StreamReader responseStream = new StreamReader(httpResponse.GetResponseStream());
            string response = responseStream.ReadToEnd();
            responseStream.Close();

            return response;
        }


    }
}
