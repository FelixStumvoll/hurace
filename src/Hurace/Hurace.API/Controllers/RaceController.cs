using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.ActiveRaceControlService.Service;
using Hurace.Core.Logic.Services.ActiveRaceService;
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
        private readonly IActiveRaceService _activeRaceService;

        public RaceController(IRaceBaseDataService baseDataService, IRaceStartListService startListService,
            IRaceStatService statService, IActiveRaceService activeRaceService)
        {
            _baseDataService = baseDataService;
            _startListService = startListService;
            _statService = statService;
            _activeRaceService = activeRaceService;
        }

        [HttpGet]
        public Task<IEnumerable<Race>> GetAll() => _baseDataService.GetAllRaces();

        [HttpGet("active")]
        public Task<IEnumerable<Race>> GetActiveRaces() => _activeRaceService.GetActiveRaces();
        
        [HttpGet("active/{id}")]
        public Task<IEnumerable<Race>> GetActiveRaceById() => _activeRaceService.GetActiveRaces();
        
        [HttpGet("active/{id}/currentSkier")]
        public Task<StartList> GetCurrentSkier(int id) => _activeRaceService.GetCurrentSkier(id);
        
        [HttpGet("active/{id}/currentSkier/splitTimes")]
        public Task<IEnumerable<TimeDifference>> GetCurrentSkierSplitTimes(int id) => 
            _activeRaceService.GetSplitTimesForCurrentSkier(id);
        
        [HttpGet("active/{id}/remainingStartList")]
        public Task<IEnumerable<StartList>> GetRemainingStartList(int id) => 
            _activeRaceService.GetRemainingStartList(id);

        [HttpGet("active/{id}/currentSkier/possiblePosition")]
        public async Task<ActionResult<int>> GetCurrentSkierPossiblePosition(int id)
        {
            var possiblePosition = await _activeRaceService.GetPossiblePositionForCurrentSkier(id);
            if (possiblePosition.HasValue) return Ok(possiblePosition.Value);
            return BadRequest();
        }

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