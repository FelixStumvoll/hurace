using System;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class Season 
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}