using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Started");
			Console.WriteLine("Getting DB Connection...");

			IDbManager db = new SqliteDbManager();
			DbConnection conn = db.getConnection();

			if (conn == null)
			{
				Console.WriteLine("Failed to get connection");
			}
			else
			{
				Console.WriteLine("\nCountry populations from SQLite Database:");
				List<Tuple<string, int>> countryPopulationsDB = GetCountryPopulationsFromDB(conn);
				WriteResultList(countryPopulationsDB);

			}

			conn.Close();

			Console.ReadLine();
		}
		static List<Tuple<string, int>> GetCountryPopulationsFromDB(DbConnection conn)
		{
			string selectPopulationCmd = "SELECT " +
													"CountryName AS Country, " +
													"SUM(Population) AS Population " +
												"FROM City c " +
			 									"JOIN Country co " +
												"ON co.CountryId = (" +
													"SELECT CountryId " +
													"FROM State s " +
													"WHERE s.StateId = c.StateId) " +
												"GROUP BY CountryId";

			SQLiteDataReader dataReader;
			SQLiteCommand cmd = (SQLiteCommand)conn.CreateCommand();

			cmd.CommandText = selectPopulationCmd;
			dataReader = cmd.ExecuteReader();

			List<Tuple<string, int>> countryPopulationsDB = new List<Tuple<string, int>>();

			while (dataReader.Read())
			{
				string countryName = dataReader.GetString(dataReader.GetOrdinal("Country"));
				int populations = Convert.ToInt32(dataReader.GetValue(dataReader.GetOrdinal("Population")));

				countryPopulationsDB.Add(Tuple.Create(countryName, populations));
			}

			dataReader.Close();

			return countryPopulationsDB;
		}

		static void WriteResultList(List<Tuple<string, int>> resultList)
		{
			foreach (var listItem in resultList)
			{
				Console.WriteLine(listItem.Item1 + " --> " + listItem.Item2);
			}
		}
	}
}
