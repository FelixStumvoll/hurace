using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private IRaceBaseDataService _baseDataService;

        public LocationController(IRaceBaseDataService baseDataService)
        {
            _baseDataService = baseDataService;
        }

        [HttpGet]
        public Task<IEnumerable<Location>> GetAll() => _baseDataService.GetLocations();
    }
}