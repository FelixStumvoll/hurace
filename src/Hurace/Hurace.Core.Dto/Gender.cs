using Hurace.Core.Dto.Attributes;


namespace Hurace.Core.Dto
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string GenderDescription { get; set; }
    }
}