using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
	class CountryService : ICountryService
	{
		public List<Tuple<string, int>> GetCombinedCountryPopulations(List<Tuple<string, int>> countryPopulationsDB)
		{
			List<Tuple<string, int>> combined = new List<Tuple<string, int>>(countryPopulationsDB);

			ConcreteStatService css = new ConcreteStatService();
			List<Tuple<string, int>> countryPopulationsStat = css.GetCountryPopulations();

			foreach (var country in countryPopulationsStat)
			{
				if (!combined.Contains(country))
				{
					combined.Add(country);
				}

			}
			return combined;
		}

		public List<Tuple<string, int>> GetCountryPopulationsFromDB(DbConnection conn)
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
			string selectPopulationWithJoinCmd = "SELECT " +
													"CountryName AS Country, " +
													"SUM(Population) AS Population " +
												"FROM City c " +
												"JOIN State s "+
												"ON s.StateId = c.StateId " +
			 									"JOIN Country co " +
												"ON co.CountryId = s.CountryId " +
												"GROUP BY co.CountryId";

			SQLiteDataReader dataReader;
			SQLiteCommand cmd;
			List<Tuple<string, int>> countryPopulationsDB = new List<Tuple<string, int>>();

			try
			{
				cmd = (SQLiteCommand)conn.CreateCommand();
				cmd.CommandText = selectPopulationWithJoinCmd;
				dataReader = cmd.ExecuteReader();


			}
			catch (SQLiteException ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}

			while (dataReader.Read())
			{
				string countryName = dataReader.GetString(dataReader.GetOrdinal("Country"));
				var populationsReaded = dataReader.GetValue(dataReader.GetOrdinal("Population"));

				int populations = Convert.ToInt32(populationsReaded);

				countryPopulationsDB.Add(Tuple.Create(countryName, populations));
			}

			dataReader.Close();

			return countryPopulationsDB;
		}
	}
}
