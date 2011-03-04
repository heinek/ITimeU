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
    /// TODO:
    /// - 
    /// </summary>
    /// 
    [TestClass]
    public class FriResImporterTest : ScenarioClass
    {
        internal static string DB_FILE = AppDomain.CurrentDomain.BaseDirectory +
            @"\..\..\..\ITimeU.Tests\Library\Import\ImporterTest.mdb";
        
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void Database_File_Exists()
        {
            System.IO.File.Exists(DB_FILE).ShouldBeTrue();
        }

        [TestMethod]
        public void Imported_Participants_Must_Be_A_List()
        {
            FriResImporter importer = null;
            List<AthleteModel> particpants = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all participants", () =>
            {
                particpants = importer.getAthletes();
            });

            Then("we should get a list of participants", () =>
            {
                particpants.ShouldBeInstanceOfType<List<AthleteModel>>();
            });
        }

        private FriResImporter createFriResImporter()
        {
            return new FriResImporter(DB_FILE);
        }

        [TestMethod]
        public void The_FriRes_Example_Db_Should_Have_Two_Participants()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all participants", () =>
            {
                athletes = importer.getAthletes();
            });

            Then("we should get a list of two specific participants", () =>
            {
                athletes.Count.ShouldBe(2);
                athletes[0].FirstName.ShouldBe("Bjarne");
                athletes[0].LastName.ShouldBe("Hansen");

                athletes[1].FirstName.ShouldBe("Yngvar");
                athletes[1].LastName.ShouldBe("Kristiansen");
            });
        }


    }

    
}

