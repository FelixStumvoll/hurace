using System;
using Hurace.Core.Common;

namespace Hurace.Core.Dto
{
    public class Skier : IDbEntity
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Included]
        public Country Country { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}