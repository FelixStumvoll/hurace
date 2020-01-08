using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.RaceBaseDataService;
using Hurace.Core.Logic.Services.RaceStartListService;
using Hurace.Core.Logic.Services.RaceStatService;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaceController : ControllerBase
    {
        private readonly IRaceBaseDataService _baseDataService;
        private readonly IRaceStartListService _startListService;
        private readonly IRaceStatService _statService;

        public RaceController(IRaceBaseDataService baseDataService, IRaceStartListService startListService,
            IRaceStatService statService)
        {
            _baseDataService = baseDataService;
            _startListService = startListService;
            _statService = statService;
        }

        [HttpGet]
        public Task<IEnumerable<Race>> GetAll() => _baseDataService.GetAllRaces();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var race = await _baseDataService.GetRaceById(id);
            if (race == null) return NotFound();
            return Ok(race);
        }

        [HttpGet("{id}/startList")]
        public Task<IEnumerable<StartList>> GetStartListForRace(int id) =>
            _startListService.GetStartListForRace(id);

        [HttpGet("{id}/ranking")]
        public Task<IEnumerable<RaceRanking>> GetRankingForRace(int id) =>
            _statService.GetRankingForRace(id);
        
        [HttpGet("{id}/winners")]
        public Task<IEnumerable<RaceRanking>> GetWinnersForRace(int id) =>
            _statService.GetWinnersForRace(id);
    }
}