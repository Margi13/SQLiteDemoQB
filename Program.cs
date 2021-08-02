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

			CountryService cs = new CountryService();

			if (conn == null)
			{
				Console.WriteLine("Failed to get connection");
			}
			else
			{
				Console.WriteLine("\nCountry populations from SQLite Database:");
				List<Tuple<string, int>> countryPopulationsDB = cs.GetCountryPopulationsFromDB(conn);
				WriteResultList(countryPopulationsDB);

				Console.WriteLine("\nCombined country populations. SQLite Database preferred:");
				List<Tuple<string, int>> countryPopulationsCombined = cs.GetCombinedCountryPopulations(countryPopulationsDB);
				WriteResultList(countryPopulationsCombined);
			}

			conn.Close();

			Console.ReadLine();
		}
		
		static void WriteResultList(List<Tuple<string, int>> resultList)
		{
			//Sort by default - alphabetically
			resultList.Sort();

			foreach (var listItem in resultList)
			{
				Console.WriteLine(listItem.Item1 + " --> " + listItem.Item2);
			}
		}
	}
}
