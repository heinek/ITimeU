using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;
using ITimeU.Tests.Library;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class ModelTestSuite : ScenarioClass
    {
        private const string CLUB_BYAASEN = "Byåsen";

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }

        [TestMethod]
        public void Run_Tests_For_ClubModel()
        {
            var t = new ClubModelTest();
            t.Two_ClubModels_With_Different_Properties_Should_Not_Equal_Each_Other();
            t.Two_ClubModels_With_Same_Properties_Should_Equal_Each_Other();
            t.When_Fetching_A_Club_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Club();
            t.When_Fetching_A_Club_That_Exists_In_The_Db_We_Should_Get_That_Club();
        }


        [TestMethod]
        public void Run_Tests_For_AthelteClassModel()
        {
            var t = new AthleteClassModelTest();
            t.When_Fetching_An_Athlete_Class_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Class();
            t.When_Fetching_An_Athlete_Class_That_Exists_In_The_Db_We_Should_Get_That_Class();
        }

        [TestMethod]
        public void Run_Tests_For_AthelteModel()
        {
            var t = new AthleteModelTest();
            t.Attempting_To_Retrieve_An_Athlete_That_Does_Not_Exist_In_The_Db_Should_Give_An_Error();
            t.It_Should_Be_Possible_To_Save_A_List_Of_Athletes_To_Database();
            t.It_Should_Be_Possible_To_Save_An_Athlete_To_The_Database();
            t.It_Should_Be_Possible_To_Save_Athlete_Details_To_The_Database();
            t.ToString_Should_Return_Firstname_And_Lastname();
        }

        [TestMethod]
        public void Run_Tests_For_FriResImporter()
        {
            var t = new FriResImporterTest();
            t.Imported_Athletes_Must_Be_A_List();
            t.Importing_A_Non_Existing_File_Should_Give_An_Error();
            t.It_Should_Be_Possible_To_Save_A_List_Of_Athletes_To_Database();
            t.No_Data_Should_Be_Saved_To_Db_When_Getting_The_List_Of_Athletes();
            t.The_FriRes_Example_Db_Should_Have_At_Least_Two_Athletes();
            t.The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information();
            t.The_FriRes_Example_Db_Should_Have_Detailed_Athlete_Information_With_Nulls();
        }
        
    }
}
