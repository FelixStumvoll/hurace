using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class RaceDao : BaseDao<Race>, IRaceDao
    {
        public RaceDao(IConnectionFactory connectionFactory) : base(connectionFactory, "hurace.race")
        {
        }

//        public override async Task<bool> UpdateAsync(Race obj)
//        {
//            return (await ExecuteAsync($"update {TableName} set " +
//                                       "seasonId=@si," +
//                                       "disciplineId=@di," +
//                                       "locationId=@li," +
//                                       "date=@d," +
//                                       "genderId=@gi " +
//                                       "raceStateId=@rs" +
//                                       "where id=@id",
//                                       ("@id", obj.Id),
//                                       ("@si", obj.SeasonId),
//                                       ("@di", obj.DisciplineId),
//                                       ("@li", obj.LocationId),
//                                       ("@d", obj.Date),
//                                       ("@gi", obj.GenderId),
//                                       ("@rs", obj.RaceStateId))) == 1;
//        }

        public override Task<bool> InsertAsync(Race obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<RaceRanking>> GetRanking(int raceId)
        {
            return QueryAsync<RaceRanking>(@"select	s.id,
            s.lastName,
            s.dateOfBirth,
            s.countryId,
            s.genderId,
            s.firstName,
            c.countryCode,
            c.name as countryName,
            g.description,
            max(td.time) as raceTime
            from hurace.TimeData as td
                join hurace.Skier as s on td.skierId = s.id
            join hurace.Country as c on s.countryId = c.id
            join hurace.Gender as g on s.genderId = g.id
            join hurace.StartList as sl on sl.raceId = @id and sl.skierId = s.id
            where td.raceId = @id and sl.startStateId = 3
            group by
            s.id,
            s.firstName,
            s.lastName,
            s.countryId,
            s.genderId,
            s.dateOfBirth,
            c.countryCode,
            c.name,
            g.description
                order by raceTime desc",
                                           new MapperConfig().AddMapping<Country>(("countryId", "Id"))
                                               .AddMapping<Gender>(("genderId", "Id")), ("@id", raceId));
        }

        public async Task<IEnumerable<StartList>> GetStartList(int raceId) =>
            await QueryAsync<StartList>(@"select
                                                sl.raceId,
                                                s.id as skierId,
                                                s.firstName,
                                                s.lastName,
                                                s.dateOfBirth,
                                                s.genderId,
                                                g.description as genderDescription,
                                                s.countryId,
                                                c.name,
                                                c.countryCode,
                                                sl.startNumber,
                                                sl.startStateId,
                                                ss.description as startStateDescription
                                                from hurace.StartList as sl
                                                join hurace.skier as s on s.id = sl.skierId
                                                join hurace.country as c on c.id = s.countryId
                                                join hurace.Gender as g on g.id = s.genderId
                                                join hurace.StartState as ss on ss.id = sl.startStateId
                                                where sl.raceId = @id
                                                order by sl.startNumber asc", new MapperConfig()
                                            .AddMapping<Country>(("countryId", "Id"))
                                            .AddMapping<Gender>(("genderId", "Id"))
                                            .AddMapping<Skier>(("skierId", "Id")), ("@id", raceId));
    }
}