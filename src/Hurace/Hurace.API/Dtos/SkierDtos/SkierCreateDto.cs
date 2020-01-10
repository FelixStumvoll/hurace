using System;

namespace Hurace.API.Dtos.SkierDtos
{
    public class SkierCreateDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        public int GenderId { get; set; }
        public string? ImageUrl { get; set; }
        
        public static implicit operator Dal.Domain.Skier(SkierCreateDto skier) => new Dal.Domain.Skier
        {
            Id = -1,
            FirstName = skier.FirstName,
            LastName = skier.LastName,
            CountryId = skier.CountryId,
            ImageUrl = skier.ImageUrl,
            DateOfBirth = skier.DateOfBirth,
            GenderId = skier.GenderId
        };
    }
}