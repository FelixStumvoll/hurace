using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Common;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, IMapper mapper) : base(
            connectionFactory, mapper, "Skier")
        {
            
            
        }

        public override Task<IEnumerable<Skier>> FindAllAsync()
        {
            return QueryAsync("select s.id, s.name, s.countryId, c.name as countryName from hurace.Skier as s join hurace.Country as c on c.id like countryId");
        }
    }
}