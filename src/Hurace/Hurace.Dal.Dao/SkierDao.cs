using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Common;
using Hurace.Dal.Common.Mapper;
using Hurace.Dal.Common.StatementBuilder;
using Hurace.Dal.Common.StatementBuilder.ConcreteStatementBuilder;
using Hurace.Dal.Dao.Base;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Dal.Dao
{
    public class SkierDao : DefaultCrudDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, StatementFactory statementFactory) : base(
            connectionFactory, "hurace.skier", statementFactory)
        {
        }

        public async Task<IEnumerable<Skier>> FindAvailableSkiersForRace(int raceId) =>
            await QueryAsync<Skier>(@"
                                                select
                                                s.id,
                                                s.firstName,
                                                s.lastName,
                                                s.genderId,
                                                s.dateOfBirth,
                                                s.countryId,
                                                c.countryCode,
                                                c.countryName,
                                                g.genderDescription
                                                from hurace.Skier as s 
                                                join hurace.gender g on g.id = s.genderId
                                                join hurace.country c on c.id = s.countryId
                                                join hurace.Race r on  r.genderId = s.genderId and r.disciplineId in 
                                                (select disciplineId from hurace.SkierDiscipline as d where d.skierId = s.id)
                                                left outer join hurace.StartList as ss on s.id = ss.skierId and ss.raceId = @ri
                                                where r.id= @ri and ss.raceId is null and ss.skierId is null and ss.startNumber is null",
                                    new MapperConfig()
                                        .AddMapping<Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                                        .AddMapping<Gender>((nameof(Skier.GenderId), nameof(Gender.Id))),
                                    ("@ri", raceId));

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
                .Join<Skier, Country>((nameof(Skier.CountryId), nameof(Country.Id)))
                .Join<Skier, Gender>((nameof(Skier.GenderId), nameof(Gender.Id)));

        public override async Task<bool> DeleteAsync(int id)
        {
            await ExecuteAsync("delete from hurace.SkierDiscipline where skierId=@si", ("@si", id));
            return await base.DeleteAsync(id);
        }

        public override async Task DeleteAllAsync()
        {
            await ExecuteAsync("delete from hurace.SkierDiscipline");
            await base.DeleteAllAsync();
        }
    }
}