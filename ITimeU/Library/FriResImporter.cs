using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITimeU.Models;
using System.Data.OleDb;
using System.Data;
using ITimeU.Logging;
using System.Web;

namespace ITimeU.Library
{
    /// <summary>
    /// Imports data from an Access database file from the FriRes system.
    /// </summary>
    public class FriResImporter
    {
        private const string DB_COL_NAME_BIRTHDAY = "Fødselsår";
        private const string DB_COL_NAME_STARTNUMBER = "Startnr";
        private const string DB_COL_NAME_NAME = "Navn";
        private const string DB_COL_NAME_CLUB = "KlubbNavn";
        private const string DB_COL_NAME_ATHLETE_CLASS = "Klasse";

        private string accessDatabaseFile;
        private OleDbConnection connection;

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
            string query = "SELECT Navn, Fødselsår, KlubbNavn, Klasse, Startnr  FROM [Deltaker]";
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
                AthleteModel athlete = createAthleteFrom(row);
                athletes.Add(athlete);
            }

            return athletes;
        }

        private AthleteModel createAthleteFrom(DataRow row)
        {
            string firstName = NameParser.FirstName((String)row[DB_COL_NAME_NAME]);
            string lastName = NameParser.LastName((String)row[DB_COL_NAME_NAME]);
            int? birthday = getIntFrom(row, DB_COL_NAME_BIRTHDAY);
            int? startNumber = getIntFrom(row, DB_COL_NAME_STARTNUMBER);
            string club = getStringFrom(row, DB_COL_NAME_CLUB);
            string athleteClass = getStringFrom(row, DB_COL_NAME_ATHLETE_CLASS);

            return new AthleteModel(firstName, lastName, birthday, new ClubModel(club), new AthleteClassModel(athleteClass), startNumber);
        }

        private int? getIntFrom(DataRow row, string rowIndex)
        {
            if (rowHasIndex(row, rowIndex))
                return (Int16)row[rowIndex];
            else
                return null;
        }

        private bool rowHasIndex(DataRow row, string rowIndex)
        {
            return row[rowIndex] != DBNull.Value;
        }

        private string getStringFrom(DataRow row, string rowIndex)
        {
            if (rowHasIndex(row, rowIndex))
                return (String)row[rowIndex];
            else
                return null;
        }

    }
}
