using System;
using System.ComponentModel.DataAnnotations;
using Hurace.Core.Dto.Attributes;
using Hurace.Core.Dto.Interfaces;

namespace Hurace.Core.Dto
{
    public class Skier : ISinglePkEntity
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        [Navigational]
        public Country? Country { get; set; }
        public int GenderId { get; set; }
        [Navigational]
        public Gender? Gender { get; set; }
    }
}