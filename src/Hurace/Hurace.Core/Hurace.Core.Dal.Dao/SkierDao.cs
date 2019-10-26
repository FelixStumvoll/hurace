using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory) : base(
            connectionFactory, "hurace.Skier")
        {
        }

        public override Task<IEnumerable<Skier>> FindAllAsync() =>
            QueryAsync(
                @"select s.id, s.firstName,
                                        s.lastName,
                                        s.genderId,
                                        s.dateOfBirth,
                                        s.countryId,
                                        c.name as countryName,
                                        c.countryCode,
                                        g.description
                                        from hurace.Skier as s
                                    join hurace.Country as c on c.id = s.countryId
                                    join hurace.Gender as g on g.id = s.genderId", new Mapper()
                    .AddMapping<Gender>(("genderId", "id")).AddMapping<Country>(
                        ("countryId", "id"), ("countryName", "name")));

        public override Task<bool> UpdateAsync(Skier obj)
        {
            throw new System.NotImplementedException();
        }
    }
}