using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class ClubModelTest : ScenarioClass
    {
        private const string CLUB_BYAASEN = "Byåsen";
        private ClubModel club;

        [TestInitialize]
        public void TestSetup()
        {
            club = null;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            if (club != null) club.DeleteFromDb();
        }

        [TestMethod]
        public void When_Fetching_A_Club_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Club()
        {
            club = null;
            int previousCount = -1;

            Given("we want to fetch a spesific club called Byåsen");

            When("we fetch a club that does not exists (Byåsen)", () =>
            {
                ClubModel.DeleteIfExists("Byåsen");
                previousCount = ClubModel.GetAll().Count;
                club = ClubModel.GetOrCreate(CLUB_BYAASEN);
            });

            Then("we shold get a club and a new DB rows should have been added", () =>
            {
                club.Name.ShouldBe(CLUB_BYAASEN);
                ClubModel.GetAll().Count.ShouldBe(previousCount + 1);
            });
        }

        [TestMethod]
        public void When_Fetching_A_Club_That_Exists_In_The_Db_We_Should_Get_That_Club()
        {
            When_Fetching_A_Club_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Club();

            club = null;
            int previousCount = 0;

            Given("we want to fetch a spesific club called Byåsen");

            When("we fetch an existing club Byåsen", () =>
            {
                previousCount = ClubModel.GetAll().Count;
                club = ClubModel.GetOrCreate(CLUB_BYAASEN);
            });

            Then("we shold get a club and no new DB rows should have been added", () =>
            {
                club.Name.ShouldBe(CLUB_BYAASEN);
                ClubModel.GetAll().Count.ShouldBe(previousCount);
            });
        }

        [TestMethod]
        public void Two_ClubModels_With_Same_Properties_Should_Equal_Each_Other()
        {
            string name = null;
            ClubModel clubModel1 = null;
            ClubModel clubModel2 = null;

            Given("we have some common properties of two clubs", () =>
            {
                // Common properties...
                name = "Trondheim";

            });

            When("we create two clubs with the same properties", () =>
            {
                clubModel1 = ClubModel.GetOrCreate(name);
                clubModel2 = ClubModel.GetOrCreate(name);
            });

            Then("the two clubs should equal each other (though not same instance)", () =>
            {
                clubModel1.ShouldBe(clubModel2);
            });

        }

        [TestMethod]
        public void Two_ClubModels_With_Different_Properties_Should_Not_Equal_Each_Other()
        {
            ClubModel clubModel1 = null;
            ClubModel clubModel2 = null;

            Given("we want to create two clubs with different names");

            When("we create two clubs with different properties", () =>
            {
                clubModel1 = ClubModel.GetOrCreate("Lade");
                clubModel2 = ClubModel.GetOrCreate("Malvik");
            });

            Then("the two clubs should not equal each other", () =>
            {
                clubModel1.ShouldNotBe(clubModel2);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_A_Club_To_The_Database()
        {
            int initialclubcount = ClubModel.GetAll().Count;
            club = null;
            Given("we want to create a club", () =>
            {
                club = new ClubModel("Test");
            });
            When("we want to save it to the database", () =>
            {
                club.Save();
            });
            Then("the number of clubs should be increased with one", () =>
            {
                ClubModel.GetAll().Count.ShouldBe(initialclubcount + 1);
            });
        }

    }
}
