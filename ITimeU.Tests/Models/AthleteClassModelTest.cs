using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class AthleteClassModelTest : ScenarioClass
    {
        private AthleteClassModel athleteClass;

        [TestInitialize]
        public void TestSetup()
        {
            athleteClass = null;
        }
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            athleteClass.Delete();
        }

        [TestMethod]
        public void When_Fetching_An_Athlete_Class_That_Exists_In_The_Db_We_Should_Get_That_Class()
        {
            When_Fetching_An_Athlete_Class_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Class();

            int previousCount = 0;

            Given("we want to fetch a spesific athlete class called G17");

            When("we fetch an existing class G17", () =>
            {
                previousCount = AthleteClassModel.GetAll().Count;
                athleteClass = AthleteClassModel.GetOrCreate("G17");
            });

            Then("we shold get an athlete class and no new DB rows should have been added", () =>
            {
                athleteClass.Name.ShouldBe("G17");
                AthleteClassModel.GetAll().Count.ShouldBe(previousCount);
            });
        }

        [TestMethod]
        public void When_Fetching_An_Athlete_Class_That_Does_Not_Exists_In_The_Db_We_Should_Create_That_Class()
        {

            int previousCount = -1;

            Given("we want to fetch a spesific class called G17");

            When("we fetch a class that does not exists (G17)", () =>
            {
                AthleteClassModel.DeleteIfExists("G17");
                previousCount = AthleteClassModel.GetAll().Count;
                athleteClass = AthleteClassModel.GetOrCreate("G17");
            });

            Then("we shold get a class and a new DB row should have been added", () =>
            {
                athleteClass.Name.ShouldBe("G17");
                AthleteClassModel.GetAll().Count.ShouldBe(previousCount + 1);
            });
        }
    }
}
