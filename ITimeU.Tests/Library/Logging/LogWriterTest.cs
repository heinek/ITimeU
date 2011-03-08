using System;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Logging;
using System.IO;

namespace ITimeU.Tests.Logging
{
    [TestClass]
    public class ParticipantModelTest : ScenarioClass
    {

        [TestMethod]
        public void We_Can_Write_A_Line_To_A_File()
        {
            LogWriter writer = null;
            string logEntry = "Hello, Log!";

            Given("we have a logwriter", () => writer = new LogWriter());

            When("we write a line of text to the log", () =>
            {
                writer.Write(logEntry);
            });

            Then("the last line of the log should be the written text", () =>
            {
                String logFile = LogWriter.getInstance().LogFile;
                string lastLine = LastLineOf(logFile);

                lastLine.ShouldBe(logEntry);
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <returns>The last line of text in the given file.</returns>
        private string LastLineOf(String file)
        {
            TextReader reader = new StreamReader(file);
            string lastLine = ReadLastLineFrom(reader);
            reader.Close();

            return lastLine;
        }

        private static string ReadLastLineFrom(TextReader reader)
        {
            string line = "";
            string lastLine = "";

            while ((line = reader.ReadLine()) != null)
            {
                lastLine = line;
            }
            return lastLine;
        }
        
        [TestMethod]
        public void TestSingleton()
        {
            LogWriter a = null;
            LogWriter b = null;

            Given("we fetch the logwriter singleton", () => a = LogWriter.getInstance());

            When("we fetch the logwriter singleton once again", () =>
            {
                b = LogWriter.getInstance();
            });

            Then("the logwriter should be the same instance", () =>
            {
                a.ShouldBeSameAs(b);
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }


    }
}
