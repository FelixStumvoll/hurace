using System;
using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Season : IDbEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}