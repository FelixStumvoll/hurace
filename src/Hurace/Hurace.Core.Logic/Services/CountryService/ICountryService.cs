using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.Services.CountryService
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountries();
    }
}