using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;


namespace Hurace.Core.Dto
{
    public class Race: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }
        public int SeasonId { get; set; }
        [Navigational]
        public Season? Season { get; set; }
        public int DisciplineId { get; set; }
        public int LocationId { get; set; }
        [Navigational]
        public Location? Location { get; set; }
        public DateTime RaceDate { get; set; }
        public int GenderId { get; set; }
        [Navigational]
        public Gender? Gender { get; set; }
        public int RaceStateId { get; set; }
        [Navigational]
        public RaceState? RaceState { get; set; }

        public string RaceDescription { get; set; } = default!;
    }
}