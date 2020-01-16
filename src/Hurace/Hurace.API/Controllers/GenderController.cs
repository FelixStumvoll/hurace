using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }
        
        [HttpGet]
        public Task<IEnumerable<Gender>> GetAll() => _genderService.GetAllGenders();
    }
}