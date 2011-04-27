using System;
using System.Collections.Generic;
using System.Linq;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;

namespace ITimeU.Tests.Models
{
    [TestClass]
    public class AthleteModelTest : ScenarioClass
    {
        private EventModel eventModel;
        private RaceModel race;
        private AthleteModel athlete;

        [TestInitialize]
        public void TestSetup()
        {
            eventModel = new EventModel("TestEvent", DateTime.Today);
            eventModel.Save();
            race = new RaceModel("TestRace", DateTime.Today);
            race.EventId = eventModel.EventId;
            race.Save();
            athlete = new AthleteModel("Test", "Testesen");
            athlete.StartNumber = 999;
            athlete.SaveToDb();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
            eventModel.Delete();
            race.Delete();
            athlete.Delete();
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_A_List_Of_Athletes_To_Database()
        {
            var athletes = new List<AthleteModel>();
            int previousAthleteDbCount = AthleteModel.GetAll().Count;

            Given("we have a list of athletes", () =>
            {
                athletes.Add(new AthleteModel("Arne", "Hansen"));
                athletes.Add(new AthleteModel("Geir", "Olsen"));
                athletes.Add(new AthleteModel("Per", "Iversen"));
            });

            When("we save the list to the database", () =>
            {
                AthleteModel.SaveToDb(athletes);
            });

            Then("the athletes in the list should be saved in the database", () =>
            {
                int athleteDbCount = AthleteModel.GetAll().Count;
                athleteDbCount.ShouldBe(previousAthleteDbCount + 3);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_A_List_Of_Athletes_WithDetails_To_Database()
        {
            var athletes = new List<AthleteModel>();
            int previousAthleteDbCount = AthleteModel.GetAll().Count;

            AthleteModel athlete1 = null;
            AthleteModel athlete2 = null;
            AthleteModel athlete3 = null;

            var clubStrindheim = ClubModel.GetOrCreate("Strindheim");
            var clubMalvik = ClubModel.GetOrCreate("Malvik");
            var classG8 = AthleteClassModel.GetOrCreate("G8");
            var classG14 = AthleteClassModel.GetOrCreate("G14");

            Given("we have a list of athletes with club and class details", () =>
            {
                

                athlete1 = new AthleteModel() {FirstName = "Ole", LastName = "Hansen", Club = clubStrindheim, AthleteClass = classG14};
                athlete2 = new AthleteModel() {FirstName = "Nils", LastName = "Olsen", Club = clubStrindheim, AthleteClass = classG8};
                athlete3 = new AthleteModel() {FirstName = "Trond", LastName = "Iversen", Club = clubMalvik, AthleteClass = classG14};

                athletes.Add(athlete1);
                athletes.Add(athlete2);
                athletes.Add(athlete3);
            });

            When("we save the list to the database", () =>
            {
                AthleteModel.SaveToDb(athletes);
            });

            Then("the athletes in the list should be saved in the database", () =>
            {
                int athleteDbCount = AthleteModel.GetAll().Count;
                athleteDbCount.ShouldBe(previousAthleteDbCount + 3);
                AthleteModel newAthlete2 = AthleteModel.GetById(athlete2.Id);
                newAthlete2.FirstName.ShouldBe("Nils");
                newAthlete2.LastName.ShouldBe("Olsen");
                newAthlete2.Club.ShouldBe(clubStrindheim);
                newAthlete2.AthleteClass.ShouldBe(classG8);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_An_Athlete_To_The_Database()
        {
            Given("we have an athlete", () =>
            {
            });

            When("we save the athlete to the database", () =>
            {
            });

            Then("it should be saved to the database", () =>
            {
                AthleteModel athleteDb = AthleteModel.GetById(athlete.Id);
                athleteDb.Id.ShouldBe(athlete.Id);
                athleteDb.FirstName.ShouldBe(athlete.FirstName);
                athleteDb.LastName.ShouldBe(athlete.LastName);
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Save_Athlete_Details_To_The_Database()
        {
            Given("we have an athlete", () =>
            {
                athlete.Birthday = 1997;
                athlete.Club = ClubModel.GetOrCreate("Strindheim");
                athlete.AthleteClass = AthleteClassModel.GetOrCreate("G14");
                athlete.StartNumber = 26;
            });

            When("we save the athlete to the database", () =>
            {
                athlete.SaveToDb();
            });

            Then("it should be saved to the database", () =>
            {
                AthleteModel athleteDb = AthleteModel.GetById(athlete.Id);
                athleteDb.Id.ShouldBe(athlete.Id);
                athleteDb.FirstName.ShouldBe(athlete.FirstName);
                athleteDb.LastName.ShouldBe(athlete.LastName);
                athleteDb.Birthday.ShouldBe(athlete.Birthday);
                athleteDb.Club.ShouldBe(athlete.Club);
                athleteDb.AthleteClass.ShouldBe(athlete.AthleteClass);
                athleteDb.StartNumber.ShouldBe(athlete.StartNumber);
            });
        }


        [TestMethod]
        public void ToString_Should_Return_Firstname_And_Lastname()
        {
            string toString = null;

            Given("we have an athlete", () =>
            {
            });

            When("we call ToString", () =>
            {
                toString = athlete.ToString();
            });

            Then("we should get firstname and lastname", () =>
            {
                toString.ShouldBe("Test Testesen");
            });
        }

        [TestMethod]
        public void Attempting_To_Retrieve_An_Athlete_That_Does_Not_Exist_In_The_Db_Should_Give_An_Error()
        {
            int athleteIdThatDoesNotExist = -1;

            Given("we have a primary key for an athlete that does not exists in the database", () =>
            {
                athleteIdThatDoesNotExist = athlete.Id;
                athlete.Delete();
            });

            When("we attempt to fetch an athlete that does not exist in the database", () =>
            {
                try
                {
                    AthleteModel.GetById(athleteIdThatDoesNotExist);
                    false.ShouldBe(true); // Fail test if exception is not thrown.
                }
                catch (ModelNotFoundException e)
                {
                    Assert.IsTrue(e.Message.Length > 0);
                }
            });

            Then("an exception should have been thrown");
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Retrieve_A_List_Of_Athletes_From_DB()
        {
            var athletes = new List<AthleteModel>();
            Given("athletes is saved to the database");

            When("we want to retrieve alle the athletes from database", () =>
            {
                athletes = AthleteModel.GetAll();
            });

            Then("the list should not be null", () => athletes.ShouldNotBeNull());
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Connect_An_Athlete_To_A_Race()
        {
            Given("we have a race and a athlete", () =>
            {
            });

            When("we want to connect a athlete to that race", () =>
            {
                athlete.ConnectToRace(race.RaceId);
            });

            Then("the list of athletes for the race should be 1", () =>
            {
                race.GetAthletes().Count().ShouldBe(1);
            });
        }

        [TestMethod]
        public void We_Should_Check_For_Identical_Startnumber_Before_Adding_A_New_Athlete()
        {
            AthleteModel newAthlete = null;

            Given("we have an athlete in the database with startnumber 999", () =>
            {
                athlete.ConnectToRace(race.RaceId);
            });

            When("we want to add a new athlete with the same startnumber", () =>
            {
                newAthlete = new AthleteModel("NewAthlete", "Test", 1980, null, null, 999);
                newAthlete.SaveToDb();
                newAthlete.ConnectToRace(race.RaceId);
            });

            Then("we should be warned if the new athlete have a startnumber that is identical to some one else in the database", () =>
            {
                AthleteModel.StartnumberExistsInDb(newAthlete.StartNumber.Value).ShouldBeTrue();
            });
        }

        [TestMethod]
        public void It_Should_Be_Possible_To_Get_All_Athletes_For_A_Club()
        {
            ClubModel newClub = null;
            AthleteModel newAthlete = null;
            List<AthleteModel> athletes = null;

            Given("we have athletes connected to a club", () =>
            {
                newClub = new ClubModel("clubTest");
                newClub.Save();
                newAthlete = new AthleteModel("Test", "Test", 1980, newClub, null, 12);
                newAthlete.SaveToDb();
            });

            When("we want to get all athletes for a club", () =>
            {
                athletes = AthleteModel.GetAthletes(newClub.Id);
            });

            Then("we should be able to get a list of athletes", () =>
            {
                athletes.ShouldNotBeNull();
            });
        }

        //[TestMethod]
        //public void It_Should_Be_Possible_To_Retrieve_A_List_Of_Athletes_Connected_To_A_Race()
        //{
        //    var athletes = new List<AthleteModel>();
        //    Given("we have a race athletes is connected to a race");

        //    When("we want to retrieve alle the athletes connected to a race", () =>
        //    {
        //        athletes = AthleteModel.GetAthletesForRace();
        //    });

        //    Then("the list should not be null", () => athletes.ShouldNotBeNull());
        //}
    }
}
