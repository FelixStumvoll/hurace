using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.CountryService
{
    [ExcludeFromCodeCoverage]
    public class CountryService : ICountryService
    {
        private readonly ICountryDao _countryDao;

        public CountryService(ICountryDao countryDao) => _countryDao = countryDao;
        
        public Task<IEnumerable<Country>> GetAllCountries() => _countryDao.FindAllAsync();
    }
}