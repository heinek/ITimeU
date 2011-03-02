using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;
using ITimeU.Library;
using System.Web;
using ITimeU.Logging;

namespace ITimeU.Tests.Library
{
    /// <summary>
    /// This class is ignored for now.
    /// </summary>

    [TestClass]
    public class FriResExporterTest : ScenarioClass
    {

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        // [TestMethod]
        public void Database_File_Exists()
        {
            System.IO.File.Exists(FriResImporterTest.DB_FILE).ShouldBeTrue();
        }

        //[TestMethod] 
        public void We_Should_Be_Able_To_Export_A_Participant()
        {
            List<AthleteModel> athletes = null;

            Given("we have two participants in a list", () =>
            {
                athletes = new List<AthleteModel>();
                athletes.Add(new AthleteModel("Arne", "Hansen"));
                athletes.Add(new AthleteModel("Per", "Olsen"));
            });

            When("we export the participant", () =>
            {
                FriResExporter exporter = new FriResExporter(FriResImporterTest.DB_FILE);
                exporter.export(athletes);
            });

            Then("the exported participants should exist in the access datbase", () =>
            {
                FriResImporter importer = new FriResImporter(FriResImporterTest.DB_FILE);
                List<AthleteModel> athletesDb = importer.getAthletes();
                athletesDb.ShouldBe(athletes);
            });
        }


    }

    
}

