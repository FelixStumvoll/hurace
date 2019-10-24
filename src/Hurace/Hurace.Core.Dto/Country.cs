namespace Hurace.Core.Dto
{
    public class Country : IDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
//        public string CountryCode { get; set; }
    }
}