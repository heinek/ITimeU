using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;
using ITimeU.Models;

namespace ITimeU.Tests.Library
{
    [TestClass]
    public class FriResImporterTest : ScenarioClass
    {

        /*
            * The following code is for importing from access db:
            * 
            * 
        // Connect to database...
        string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\frires\demo\Demo.mdb";
        OleDbConnection myConnection = new OleDbConnection(connectionString);

        // Execute query...
        string query = "select * from [Info - participant]";
        OleDbDataAdapter myAdapter = new OleDbDataAdapter(query, myConnection);

        // Retrieve results...
        DataSet dbData = new DataSet();
        myAdapter.Fill(dbData);
        DataTable table = dbData.Tables[0];

        // Print results...
        Console.WriteLine("Query 1");
        foreach (DataRow row in table.Rows)
        {
            Console.WriteLine("ParticipantID: {0}\tName:{1}", row["participantID"], row["Name"]);
        }

        myConnection.Close();
             
        * This test is a first step for utilizing this code in a BDD manner.
        */

        [TestMethod]
        public void Test_Import_Participants()
        {
            FriResImporter importer = null;
            List<ParticipantModel> particpants = null;

            Given("we have a importer for the FriRes system", () => 
            {
                importer = new FriResImporter("Import/ImporterTest.mdb");
            });

            When("we import participants", () =>
            {
                particpants = importer.getParticipants();
            });

            Then("we should get a list of particiapnts", () =>
            {
                particpants.ShouldBeInstanceOfType<List<ParticipantModel>>();
            });
        }

        [TestMethod]
        public void The_FriRes_Db_Should_Have_Two_Participants()
        {
            FriResImporter importer = null;
            List<ParticipantModel> particpants = null;

            Given("we have a importer for the FriRes system", () => 
            {
                importer = new FriResImporter("Import/ImporterTest.mdb");
            });

            When("we import participants", () =>
            {
                particpants = importer.getParticipants();
            });

            Then("we should get a list of particiapnts", () =>
            {
                
            });
        }
    }
}
