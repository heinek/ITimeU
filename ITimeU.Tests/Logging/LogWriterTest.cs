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
            var lw = new LogWriter();
            lw.Write("Hello, Log!");
            true.ShouldBeTrue();
        }
        
        [TestMethod]
        public void A_Line_Written_To_The_Log_Is_Appended_To_File_Log()
        {
            string file = "LogTest.txt";
            string hello = "Hello, Log!";

            var lw = new LogWriter(file);
            lw.Write(hello);

            StreamReader reader = new StreamReader(file);
            String read = reader.ReadLine();
            reader.Close();

            read.ShouldBe(hello);
        }

        [TestMethod]
        public void TestSingleton()
        {
            LogWriter a = LogWriter.getInstance();
            LogWriter b = LogWriter.getInstance();

            a.ShouldBe(b);
        }

        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
