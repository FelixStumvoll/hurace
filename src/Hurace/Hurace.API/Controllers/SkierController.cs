using System.Collections.Generic;
using System.Threading.Tasks;
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

        // [HttpGet("{id}/results")]
        // public async Task<IEnumerable<RaceRanking>> GetRankingsForSkier()
        // {
        //        
        // }
    }
}