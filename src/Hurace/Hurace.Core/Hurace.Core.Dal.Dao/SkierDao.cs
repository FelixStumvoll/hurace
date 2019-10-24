using System.Threading.Tasks;
using AutoMapper;
using Hurace.Core.Common;
using Hurace.Core.Dto;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>
    {
        public SkierDao(IConnectionFactory connectionFactory, Mapper mapper) : base(
            connectionFactory, mapper, "Skier")
        {
        }
    }
}