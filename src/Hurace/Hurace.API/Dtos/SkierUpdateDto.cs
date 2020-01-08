using Hurace.Dal.Domain;

namespace Hurace.API.Dtos
{
    public class SkierUpdateDto : SkierCreateDto
    {
        public int Id { get; set; }
        
        public static implicit operator Skier(SkierUpdateDto skier) => new Skier
        {
            Id = skier.Id,
            FirstName = skier.FirstName,
            LastName = skier.LastName,
            CountryId = skier.CountryId,
            ImageUrl = skier.ImageUrl,
            DateOfBirth = skier.DateOfBirth,
            GenderId = skier.GenderId
        };
    }
}