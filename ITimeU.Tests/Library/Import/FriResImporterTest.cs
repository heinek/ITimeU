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
using System.IO;

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
        public void Imported_Athletes_Must_Be_A_List()
        {
            FriResImporter importer = null;
            List<AthleteModel> particpants = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes", () =>
            {
                LogWriter.getInstance().Write(
                     "Imported_Athletes_Must_Be_A_List, getAthletes()");

                particpants = importer.getAthletes();
            });

            Then("we should get a list of athletes", () =>
            {
                particpants.ShouldBeInstanceOfType<List<AthleteModel>>();
            });
        }

        private FriResImporter createFriResImporter()
        {
            return new FriResImporter(DB_FILE);
        }

        [TestMethod]
        public void The_FriRes_Example_Db_Should_Have_At_Least_Two_Athletes()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes", () =>
            {
                LogWriter.getInstance().Write(
                  "The_FriRes_Example_Db_Should_Have_At_Least_Two_Athletes, getAthletes()");
                athletes = importer.getAthletes();
            });

            Then("we should get a list of two specific athletes", () =>
            {
                athletes[0].FirstName.ShouldBe("Bjarne");
                athletes[0].LastName.ShouldBe("Hansen");

                athletes[1].FirstName.ShouldBe("Yngvar");
                athletes[1].LastName.ShouldBe("Kristiansen");
            });
        }


        [TestMethod]
        public void The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes", () =>
            {
                LogWriter.getInstance().Write(
                    "The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information, getAthletes()");
                athletes = importer.getAthletes();
            });

            Then("we should get an athlete with detailed information", () =>
            {
                athletes[2].FirstName.ShouldBe("Test");
                athletes[2].LastName.ShouldBe("Deltaker");
                athletes[2].Birthday.ShouldBe(1996);
                athletes[2].Club.ShouldBe(new ClubModel("Lade"));
                athletes[2].AthleteClass.ShouldBe(new AthleteClassModel("Jenter 15"));
                athletes[2].StartNumber.ShouldBe(99);
            });
        }

        [TestMethod]
        public void The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information_With_Nulls()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes", () =>
            {
                LogWriter.getInstance().Write(
                   "The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information_With_Nulls, getAthletes()");

                athletes = importer.getAthletes();
            });

            Then("we should get an athlete with detailed information", () =>
            {
                athletes[3].FirstName.ShouldBe("Ole");
                athletes[3].LastName.ShouldBe("Hansen");
                athletes[3].Birthday.ShouldBe(null);
                athletes[3].Club.ShouldBe(new ClubModel("Rognan"));
                athletes[3].AthleteClass.ShouldBe(new AthleteClassModel("Jenter 30"));
                athletes[3].StartNumber.ShouldBe(100);
            });
        }

        [TestMethod]
        public void No_Data_Should_Be_Saved_To_Db_When_Getting_The_List_Of_Athletes()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;
            int previousAthleteCount = -1;
            int previousAthleteClassCount = -1;
            int previousClubCount = -1;

            Given("we have a importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes", () =>
            {
                previousAthleteCount = AthleteModel.GetAll().Count;
                previousAthleteClassCount = AthleteClassModel.GetAll().Count;
                previousClubCount = ClubModel.GetAll().Count;

                LogWriter.getInstance().Write(
                     "No_Data_Should_Be_Saved_To_Db_When_Getting_The_List_Of_Athletes, getAthletes()");

                athletes = importer.getAthletes();
            });

            Then("the number of rows for the relevant tables should be the same", () =>
            {
                AthleteModel.GetAll().Count.ShouldBe(previousAthleteCount);
                AthleteClassModel.GetAll().Count.ShouldBe(previousAthleteClassCount);
                ClubModel.GetAll().Count.ShouldBe(previousClubCount);
            });
        }

        [TestMethod]
        public void Importing_A_Non_Existing_File_Should_Give_An_Error()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have a FriRes importer pointing to a non existing database file", () =>
            {
                importer = new FriResImporter("nonexstingfile.ext");
            });

            When("we attempt import all athletes from the file", () =>
            {
                try
                {
                    LogWriter.getInstance().Write(
                         "Importing_A_Non_Existing_File_Should_Give_An_Error, getAthletes()");

                    athletes = importer.getAthletes();
                    false.ShouldBeTrue();
                }
                catch (FileNotFoundException) {
                    // The call above should throw this exception.
                }
            });

            Then("we should have got an exception");
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_A_List_Of_Athletes_To_Database()
        {
            FriResImporter importer = null;
            List<AthleteModel> athletes = null;

            Given("we have an importer for the FriRes system", () =>
            {
                importer = createFriResImporter();
            });

            When("we import all athletes and save them to the database", () =>
            {
                athletes = importer.getAthletes();
                FriResImporter.SaveAthletesToDatabase(athletes);
            });

            Then("the database should contain the saved athletes", () =>
            {
                foreach (AthleteModel athlete in athletes)
                    AthleteModel.GetById(athlete.Id); // Should not throw an exception.
            });
        }

    }

    
}

