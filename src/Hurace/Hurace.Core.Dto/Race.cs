using System;
using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class Race
    {
        [Key]
        public int Id { get; set; }
        public int SeasonId { get; set; }
        [Attributes.Navigational]
        public Season? Season { get; set; }
        public int DisciplineId { get; set; }
        public int LocationId { get; set; }
        [Attributes.Navigational]
        public Location? Location { get; set; }
        public DateTime RaceDate { get; set; }
        public int GenderId { get; set; }
        [Attributes.Navigational]
        public Gender? Gender { get; set; }
        public int RaceStateId { get; set; }
        [Attributes.Navigational]
        public RaceState? RaceState { get; set; }

        public string RaceDescription { get; set; }
    }
}