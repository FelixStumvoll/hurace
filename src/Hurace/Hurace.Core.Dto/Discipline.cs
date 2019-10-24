using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Discipline : IDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}