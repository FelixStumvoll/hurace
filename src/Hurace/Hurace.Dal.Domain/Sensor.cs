using System.ComponentModel.DataAnnotations;
using Hurace.Dal.Domain.Attributes;
using Hurace.Dal.Domain.Interfaces;

namespace Hurace.Dal.Domain
{
    public class Sensor: ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string SensorDescription { get; set; } = default!;
        public int RaceId { get; set; }
        public int SensorNumber { get; set; }
        [Navigational]
        public Race? Race { get; set; }
    }
}