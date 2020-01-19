using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.API.Dtos;
using Hurace.API.Dtos.RaceRankingDtos;
using Hurace.API.Dtos.StartListDtos;
using Hurace.API.Dtos.TimeDifferenceDtos;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaceController : ControllerBase
    {
        private readonly IRaceService _raceService;
        private readonly IRaceStartListService _startListService;
        private readonly IRaceStatService _statService;
        private readonly IActiveRaceService _activeRaceService;

        public RaceController(IRaceService raceService, IRaceStartListService startListService,
            IRaceStatService statService, IActiveRaceService activeRaceService)
        {
            _raceService = raceService;
            _startListService = startListService;
            _statService = statService;
            _activeRaceService = activeRaceService;
        }

        [HttpGet]
        public Task<IEnumerable<Race>> GetAll() => _raceService.GetAllRaces();

        [HttpGet("active")]
        public Task<IEnumerable<Race>> GetActiveRaces() => _activeRaceService.GetActiveRaces();
        
        [HttpGet("active/{id}/currentSkier")]
        public Task<StartList?> GetCurrentSkier(int id) => _activeRaceService.GetCurrentSkier(id);
        
        [HttpGet("active/{id}/currentSkier/splitTimes")]
        public async Task<IEnumerable<TimeDifferenceDto>> GetCurrentSkierSplitTimes(int id) =>
            (await _activeRaceService.GetSplitTimesForCurrentSkier(id)).Select(TimeDifferenceDto.FromTimeDifference);
        
        [HttpGet("active/{id}/remainingStartList")]
        public async Task<IEnumerable<StartListForRaceDto>> GetRemainingStartList(int id) =>
            (await _activeRaceService.GetRemainingStartList(id)).Select(StartListForRaceDto.FromStartList);

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
            var race = await _raceService.GetRaceById(id);
            if (race == null) return NotFound();
            return Ok(race);
        }

        [HttpGet("{id}/startList")]
        public async Task<IEnumerable<StartListForRaceDto>> GetStartListForRace(int id) =>
            (await _startListService.GetStartListForRace(id)).Select(StartListForRaceDto.FromStartList);

        [HttpGet("{id}/ranking")]
        public async Task<IEnumerable<RaceRankingDto>> GetRankingForRace(int id) =>
            (await _statService.GetRankingForRace(id)).Select(RaceRankingDto.FromRaceRanking);
        
        [HttpGet("{id}/winners")]
        public async Task<IEnumerable<RaceRankingDto>> GetWinnersForRace(int id) =>
            (await _statService.GetWinnersForRace(id)).Select(RaceRankingDto.FromRaceRanking);
    }
}