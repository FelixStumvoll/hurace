using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class Season : ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}