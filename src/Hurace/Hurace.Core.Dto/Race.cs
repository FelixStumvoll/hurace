using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Race
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        [Navigational]
        public Season Season { get; set; }
        public int DisciplineId { get; set; }
        public int LocationId { get; set; }
        [Navigational]
        public Location Location { get; set; }
        public DateTime DateTime { get; set; }
        public int GenderId { get; set; }
        [Navigational]
        public Gender Gender { get; set; }
    }
}