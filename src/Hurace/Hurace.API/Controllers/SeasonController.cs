using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Services.SeasonService;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        [HttpGet]
        public Task<IEnumerable<Season>> GetAll() => _seasonService.GetAllSeasons();

        [HttpPut]
        public async Task<ActionResult> InsertOrUpdate(Season season)
        {
            if (await _seasonService.InsertOrUpdateSeason(season)) return Ok();
            return BadRequest();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetById(int id)
        {
            var season = await _seasonService.GetSeasonById(id);
            return season == null ? (ActionResult<Season>) NotFound() : Ok(season);
        }
        
        [HttpGet("{id}/races")]
        public Task<IEnumerable<Race>> GetAllRaces(int id) => _seasonService.GetRacesForSeason(id);
    }
}