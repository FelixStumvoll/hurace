using System;
using Hurace.Core.Dto.Attributes;

namespace Hurace.Core.Dto
{
    public class Skier 
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }
        [Navigational]
        public Country? Country { get; set; }
        public int GenderId { get; set; }
        [Navigational]
        public Gender? Gender { get; set; }
    }
}