namespace Hurace.Core.Dto
{
    public class Location : IDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}