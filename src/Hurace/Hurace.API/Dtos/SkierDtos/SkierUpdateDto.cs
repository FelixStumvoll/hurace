namespace Hurace.API.Dtos.SkierDtos
{
    public class SkierUpdateDto : SkierCreateDto
    {
        public int Id { get; set; }
        
        public static implicit operator Dal.Domain.Skier(SkierUpdateDto skier) => new Dal.Domain.Skier
        {
            Id = skier.Id,
            FirstName = skier.FirstName,
            LastName = skier.LastName,
            CountryId = skier.CountryId,
            ImageUrl = skier.ImageUrl,
            DateOfBirth = skier.DateOfBirth,
            GenderId = skier.GenderId,
            Retired = skier.Retired
        };
    }
}