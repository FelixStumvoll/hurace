using System;
using Hurace.Dal.Domain;

namespace Hurace.API.Dtos
{
    public class SkierCreateDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        public int GenderId { get; set; }
        public string? ImageUrl { get; set; }
        
        public static implicit operator Skier(SkierCreateDto skier) => new Skier
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