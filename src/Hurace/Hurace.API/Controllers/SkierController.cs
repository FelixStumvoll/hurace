using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.API.Dtos;
using Hurace.API.Dtos.Skier;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Services.SkierService;
using Hurace.Dal.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Hurace.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkierController : ControllerBase
    {
        private readonly ISkierService _skierService;

        public SkierController(ISkierService skierService)
        {
            _skierService = skierService;
        }

        [HttpGet]
        public Task<IEnumerable<Skier>> GetAll() => _skierService.GetAllSkiers();

        [HttpGet("{id}")]
        public async Task<ActionResult<Skier>> GetById(int id)
        {
            var skier = await _skierService.GetSkierById(id);
            if (skier == null) return NotFound();
            return skier;
        }

        [HttpGet("{id}/disciplines")]
        public Task<IEnumerable<Discipline>> GetDisciplinesForSkier(int id) => 
            _skierService.GetDisciplinesForSkier(id);

        [HttpPut("{id}/disciplines")]
        public async Task<ActionResult> PutDisciplinesForSkier(int id, IEnumerable<int> disciplines)
        {
            if (await _skierService.UpdatePossibleDisciplines(id, disciplines)) return Ok();
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutSkier(SkierUpdateDto skier)
        {
            if (await _skierService.UpdateSkier(skier)) return Ok();
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> PutSkier(SkierCreateDto skier)
        {
            var skierId = await _skierService.CreateSkier(skier);
            if(!skierId.HasValue) return BadRequest();
            
            var retVal = await _skierService.GetSkierById(skierId.Value);
            return Created($"/skier/{skierId.Value}", retVal!);
        }
        
        
        
        // [HttpGet("{id}/results")]
        // public async Task<IEnumerable<RaceRanking>> GetRankingsForSkier()
        // {
        //        
        // }
    }
}