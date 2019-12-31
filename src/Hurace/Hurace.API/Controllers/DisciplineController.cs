using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplineController : ControllerBase
    {
        private readonly IRaceBaseDataService _baseDataService;

        public DisciplineController(IRaceBaseDataService baseDataService)
        {
            _baseDataService = baseDataService;
        }

        [HttpGet]
        public Task<IEnumerable<Discipline>> GetAll() => _baseDataService.GetAllDisciplines();
    }
}