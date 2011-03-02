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
    public class FriResExporter
    {
        private string accessDatabaseFile;

        public FriResExporter(string accessDatabaseFile)
        {
            this.accessDatabaseFile = accessDatabaseFile;
        }


        public void export(List<AthleteModel> participants)
        {
            /*
            // Connect to database...
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + accessDatabaseFile;
            OleDbConnection connection = new OleDbConnection(connectionString);

            // Execute query...
            string query = @"
                INSERT INTO Deltaker ([Navn], [Fødselsår])
                VALUES ('Per Ivar Jakobsen', '1956');
                ";

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand(query, connection);



            adapter.SelectCommand = command;
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);

            var ds = new DataSet("Deltaker");
            
            DataRow newParticipantRow = ds.Tables["Deltaker"].NewRow();
            newParticipantRow["Navn"] = "Geir Pedersen";

            ds.Tables["Deltaker"].Rows.Add(newParticipantRow);

            DataTable dt = ds.Tables[0];
            adapter.Update(dt);

            // DataTable table = new DataTable("Deltaker");
            // table.Columns.Add("Navn");
            // table.Columns.Add("Fødselsår");
            // table.Rows.Add("Arne Pedersen", 1932);
            // adapter.Update(table);
            

            connection.Close();
            */
        }
    }
}
