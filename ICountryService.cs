using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
	public interface ICountryService
	{
		List<Tuple<string, int>> GetCountryPopulationsFromDB(DbConnection conn);
		List<Tuple<string, int>> GetCombinedCountryPopulations(List<Tuple<string, int>> countryPopulationsDB);
	}
}
