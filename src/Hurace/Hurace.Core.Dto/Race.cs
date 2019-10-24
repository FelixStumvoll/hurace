using System;

namespace Hurace.Core.Dto
{
    public class Race : IDbEntity
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public int DisciplineId { get; set; }
        public int LocationId { get; set; }
        public DateTime DateTime { get; set; }
        public string Gender { get; set; }
    }
}