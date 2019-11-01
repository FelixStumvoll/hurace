using Hurace.Core.Dto.Util;

namespace Hurace.Core.Dto
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string GenderDescription { get; set; }
    }
}