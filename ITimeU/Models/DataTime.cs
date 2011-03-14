using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITimeU.Models;
using System.Data.OleDb;
using System.Data;
using ITimeU.Logging;
using System.Web;

namespace ITimeU.Tests.Library
{
    class FriResImporter
    /// Imports data from an Access database file from the FriRes system.
    /// </summary>
    public class FriResImporter
    {
        private string accessDatabaseFile;
        OleDbConnection connection;

        public FriResImporter(string accessDatabaseFile)
        {
            this.accessDatabaseFile = accessDatabaseFile;
        }

        /// <summary>
        /// Reads all athletes from the database.
        /// </summary>
        /// <returns></returns>
        public List<AthleteModel> getAthletes()
        {
            connection = connectToDb();
            OleDbDataAdapter adapterWithAthletes = selectAllAthletes();
            List<AthleteModel> participants = fetchAthletes(adapterWithAthletes);
            connection.Close();

            return participants;
        }

        private OleDbConnection connectToDb()
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + accessDatabaseFile;
            return new OleDbConnection(connectionString);
        }

        private OleDbDataAdapter selectAllAthletes()
        {
            string query = "SELECT * FROM [Deltaker]";
            return new OleDbDataAdapter(query, connection);
        }

        private List<AthleteModel> fetchAthletes(OleDbDataAdapter adapter)
        {
            DataTable table = fetchTableFrom(adapter);
            return fetchAthletesFrom(table);
        }

        private DataTable fetchTableFrom(OleDbDataAdapter adapter)
        {
            DataSet dbData = new DataSet();
            adapter.Fill(dbData);
            return dbData.Tables[0];
        }

        private List<AthleteModel> fetchAthletesFrom(DataTable table)
        {
            List<AthleteModel> athletes = new List<AthleteModel>();

            foreach (DataRow row in table.Rows)
            {
                string firstName = NameParser.FirstName((String)row["Navn"]);
                string surName = NameParser.LastName((String)row["Navn"]);
                string fullName = firstName + " " + surName;
                AthleteModel pm = new AthleteModel(firstName, surName);
                athletes.Add(pm);
            }

            return athletes;
        }

    }
}
