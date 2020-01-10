using System;

namespace Hurace.API.Dtos.SeasonDtos
{
    public class SeasonCreateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public static implicit operator Dal.Domain.Season(SeasonCreateDto season) =>
            new Dal.Domain.Season {StartDate = season.StartDate, EndDate = season.EndDate};
    }
}