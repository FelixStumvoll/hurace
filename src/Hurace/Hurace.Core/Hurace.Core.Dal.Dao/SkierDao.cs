using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.skier", statementFactory)
        {
        }

        public async Task<IEnumerable<Discipline>> GetPossibleDisciplinesForSkier(int skierId) =>
            await QueryAsync<Discipline>(@"select d.id, d.disciplineName 
                                                    from hurace.Discipline as d
                                                    join hurace.SkierDiscipline as sd on d.id = sd.disciplineId
                                                    where sd.skierId = @si",
                                         queryParams: ("@si", skierId));

        public async Task<bool> InsertPossibleDisciplineForSkier(int skierId, int disciplineId) =>
            await ExecuteAsync("insert into hurace.SkierDiscipline values(@si, @di)", ("@si", skierId),
                               ("@di", disciplineId));

        public async Task<bool> DeletePossibleDisciplineForSkier(int skierId, int disciplineId) =>
            await ExecuteAsync("delete from hurace.SkierDiscipline where skierId=@si and disciplineId=@di",
                               ("@si", skierId), ("@di", disciplineId));


        private protected override SelectStatementBuilder<Skier> DefaultSelectQuery() =>
            StatementFactory
                .Select<Skier>()
                .Join<Skier, Country>(("countryId", "id"))
                .Join<Skier, Gender>(("genderId", "id"));

        public override async Task<bool> DeleteAsync(int id)
        {
            await ExecuteAsync("delete from hurace.SkierDiscipline where skierId=@si", ("@si", id));
            return await base.DefaultDelete(id);
        }

        public override async Task DeleteAllAsync()
        {
            await ExecuteAsync("delete from hurace.SkierDiscipline");
            await base.DefaultDeleteAll();
        }
    }
}