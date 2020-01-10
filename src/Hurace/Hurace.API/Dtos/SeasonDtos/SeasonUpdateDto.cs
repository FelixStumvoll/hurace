namespace Hurace.API.Dtos.SeasonDtos
{
    public class SeasonUpdateDto : SeasonCreateDto
    {
        public int Id { get; set; }
        
        public static implicit operator Dal.Domain.Season(SeasonUpdateDto season) =>
            new Dal.Domain.Season {Id = season.Id, StartDate = season.StartDate, EndDate = season.EndDate};
    }
}