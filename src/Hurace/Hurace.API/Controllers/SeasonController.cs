using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.API.Dtos;
using Hurace.API.Dtos.SeasonDtos;
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

        [HttpPut("${id}")]
        public async Task<ActionResult> UpdateSeason(SeasonUpdateDto season)
        {
            if (await _seasonService.UpdateSeason(season)) return Ok();
            return BadRequest();
        }
        
        [HttpPut]
        public async Task<ActionResult> InsertSeason(SeasonCreateDto season)
        {
            var seasonId = await _seasonService.InsertSeason(season);
            if (!seasonId.HasValue) return BadRequest();
            var s = _seasonService.GetSeasonById(seasonId.Value);
            return Created($"season/{seasonId.Value}", s);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSeason(int id)
        {
            if (await _seasonService.DeleteSeason(id)) return Ok();
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