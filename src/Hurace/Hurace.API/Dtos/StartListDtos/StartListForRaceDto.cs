using Hurace.Dal.Domain;

namespace Hurace.API.Dtos.StartListDtos
{
    public class StartListForRaceDto
    {
        public int StartNumber { get; set; }
        public Skier? Skier { get; set; }
        public int StartStateId { get; set; }


        private StartListForRaceDto(int startNumber, Skier? skier, int startStateId)
        {
            StartNumber = startNumber;
            Skier = skier;
            StartStateId = startStateId;
        }

        public static StartListForRaceDto FromStartList(StartList sl) => new StartListForRaceDto(
            sl.StartNumber,
            sl.Skier,
            sl.StartStateId
        );
    }
}