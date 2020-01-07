using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.CountryService;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IRaceBaseDataService _baseDataService;
        private readonly ICountryService _countryService;

        public CountryController(IRaceBaseDataService baseDataService, ICountryService countryService)
        {
            _baseDataService = baseDataService;
            _countryService = countryService;
        }

        [HttpGet]
        public Task<IEnumerable<Country>> GetAll() => _countryService.GetAllCountries();
    }
}