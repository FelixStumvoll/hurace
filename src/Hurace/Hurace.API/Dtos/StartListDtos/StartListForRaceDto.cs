using Hurace.Dal.Domain;

namespace Hurace.API.Dtos.StartListDtos
{
    public class StartListForRaceDto
    {
        public int StartNumber { get; set; }
        public Skier Skier { get; set; }
        public int StartStateId { get; set; }
        
        
        public static StartListForRaceDto FromStartList(StartList sl) => new StartListForRaceDto
        {
            Skier = sl.Skier,
            StartNumber = sl.StartNumber,
            StartStateId = sl.StartStateId
        };
    }
}