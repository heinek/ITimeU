using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Library;

namespace ITimeU.Tests.Library
{
    [TestClass]
    public class NameParserTest : ScenarioClass
    {
        private string firstName = null;
        private string lastName = null;
        private string fullName = null;

        [TestCleanup]
        public void TestCleanup()
        {
            StartScenario();
        }
        
        [TestMethod]
        public void Test_Parse_First_Name()
        {
            String firstNameResult = null;

            Given("we have a first name, middle name and a last name", () =>
            {
                firstName = "Per Arne";
                lastName = "Hansen";
                fullName = firstName + " " + lastName;
            });

            When("we fetch the last name from the full name", () =>
            {
                firstNameResult = NameParser.FirstName(fullName);
            });

            Then("the parsed result should be exactly the first name", () =>
            {
                firstNameResult.ShouldBe(firstName);
            });
        }

        [TestMethod]
        public void Test_Parse_Last_Name()
        {
            String lastNameResult = null;

            Given("we have a first name, middle name and a last name", () =>
            {
                firstName = "Per Arne";
                lastName = "Hansen";
                fullName = firstName + " " + lastName;
            });

            When("we fetch the last name from the full name", () =>
            {
                lastNameResult = NameParser.LastName(fullName);
            });

            Then("the parsed result should be exactly the last name", () =>
            {
                lastNameResult.ShouldBe(lastName);
            });
        }

        [TestMethod]
        public void Test_Lots_Of_First_Name_Variations()
        {
            Given("we have a first name", () =>
            {
                firstName = "Per";
            });

            When("we fetch the first name from variations of the first name", () =>
            {
                NameParser.FirstName(firstName).ShouldBe(firstName);
                NameParser.FirstName(firstName + " ").ShouldBe(firstName);
                NameParser.FirstName(firstName + "  ").ShouldBe(firstName);
                NameParser.FirstName(" " + firstName + "  ").ShouldBe(firstName);
                NameParser.FirstName("  " + firstName + "  ").ShouldBe(firstName);
            });

            Then("the parsed result should be exactly the first name");
        }

        [TestMethod]
        public void Test_Lots_Of_First_Name_With_Middle_Name_Variations()
        {
            Given("we have a first name, middle name and a last name", () =>
            {
                firstName = "Per Arne";
                lastName = "Hansen";
                fullName = firstName + " " + lastName;
            });

            When("we fetch the first name from variations of the full name", () =>
            {
                NameParser.FirstName(fullName).ShouldBe(firstName);
                NameParser.FirstName(fullName + " ").ShouldBe(firstName);
                NameParser.FirstName(fullName + "  ").ShouldBe(firstName);
                NameParser.FirstName(" " + fullName + "  ").ShouldBe(firstName);
                NameParser.FirstName("  " + fullName + "  ").ShouldBe(firstName);
            });

            Then("the parsed result should be exactly the first name");
        }

        [TestMethod]
        public void Test_Lots_Of_Last_Name()
        {
            Given("we have a first name, middle name and a last name", () =>
            {
                firstName = "Per Arne";
                lastName = "Hansen";
                fullName = firstName + " " + lastName;
            });

            When("we fetch the last name from variations of the full name", () =>
            {
                NameParser.LastName(fullName).ShouldBe(lastName);
                NameParser.LastName(fullName + " ").ShouldBe(lastName);
                NameParser.LastName(fullName + "  ").ShouldBe(lastName);
                NameParser.LastName(" " + fullName + "  ").ShouldBe(lastName);
                NameParser.LastName("  " + fullName + "  ").ShouldBe(lastName);
            });

            Then("the parsed result should be the correct last name");
        }
    }
}
