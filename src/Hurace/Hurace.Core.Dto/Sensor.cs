using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class Sensor: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string SensorDescription { get; set; } = default!;
        public int RaceId { get; set; }
        [Navigational]
        public Race? Race { get; set; }
    }
}