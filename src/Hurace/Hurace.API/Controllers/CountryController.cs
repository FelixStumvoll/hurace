using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController( ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public Task<IEnumerable<Country>> GetAll() => _countryService.GetAllCountries();
    }
}