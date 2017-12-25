using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using CsvExport.Common;
using CsvExport.Models;

namespace CsvExport.DataAccess
{
    internal class ExportDataAccess
    {
        /// <summary>
        /// Gets the data from database.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <returns></returns>
        internal static IEnumerable<DataModel> GetDataFromDatabase(string sqlQuery)
        {
            IList<DataModel> records = new List<DataModel>();
            using (var sqlConnection = new SQLiteConnection(Constants.ConnectoinString))
            {
                var command = new SQLiteCommand(sqlQuery, sqlConnection);

                sqlConnection.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    records.Add(new DataModel
                    {
                        TransId = Convert.ToString(reader[0], CultureInfo.CurrentCulture),
                        AccountName = reader.GetString(1),
                        TransCode = reader.GetString(2),
                        Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                        CategName = reader.IsDBNull(4) ? null : reader.GetString(4),
                        SubCategName = reader.IsDBNull(5) ? null : reader.GetString(5),
                        PayeeName = reader.IsDBNull(6) ? null : reader.GetString(6),
                        TransAmount = reader.GetDecimal(7),
                        TransDate = SQLiteConvert.ToString(reader.GetDateTime(8), SQLiteDateFormats.Default, DateTimeKind.Local, "yyyy/MM/dd")
                    });
                }
            }
            return records;
        }
    }
}