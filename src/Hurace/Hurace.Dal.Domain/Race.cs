using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class Race : ISinglePkEntity
    {
        [Key] public int Id { get; set; }
        public string RaceDescription { get; set; } = default!;
        public int SeasonId { get; set; }
        [Navigational] public Season? Season { get; set; }
        public int DisciplineId { get; set; }
        [Navigational] public Discipline Discipline { get; set; }
        public int LocationId { get; set; }
        [Navigational] public Location? Location { get; set; }
        public DateTime RaceDate { get; set; }
        public int GenderId { get; set; }
        [Navigational] public Gender? Gender { get; set; }
        public int RaceStateId { get; set; }
        [Navigational] public RaceState? RaceState { get; set; }
    }
}