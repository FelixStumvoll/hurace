using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Common.Mapper;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.Core.Dal.Dao
{
    public class SkierDao : BaseDao<Skier>, ISkierDao
    {
        public SkierDao(IConnectionFactory connectionFactory, QueryFactory queryFactory) : base(
            connectionFactory, "hurace.skier", queryFactory)
        {
        }

        public override Task<bool> UpdateAsync(Skier obj)
        {
            throw new System.NotImplementedException();
        }


        public override async Task<IEnumerable<Skier>> FindAllAsync() =>
            await GeneratedQueryAsync(_queryFactory
                                          .Select<Skier>()
                                          .Join<Skier, Country>(("countryId", "id"))
                                          .Join<Skier, Gender>(("genderId", "id"))
                                          .Build());

//        public override Task<IEnumerable<Skier>> FindAllAsync() =>
//            QueryAsync<Skier>(
//                @"select s.id, s.firstName,
//                                        s.lastName,
//                                        s.genderId,
//                                        s.dateOfBirth,
//                                        s.countryId,
//                                        c.countryName,
//                                        c.countryCode,
//                                        g.genderDescription
//                                        from hurace.Skier as s
//                                    join hurace.Country as c on c.id = s.countryId
//                                    join hurace.Gender as g on g.id = s.genderId",
//                new MapperConfig()
//                    .AddMapping<Gender>(("genderId", "id"))
//                    .AddMapping<Country>(("countryId", "id"), ("countryName", "name")));

        public override Task<Skier> FindByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

//        public override async Task<bool> UpdateAsync(Skier obj) =>

//            (await ExecuteAsync(

//                $"update {TableName} set " +

//                "firstName=@fn," +

//                "lastName=@ln," +

//                "dateOfBirth=@dob," +

//                "countryId=@ci,genderId=@gi " +

//                "where id=@id",

//                ("@fn", obj.FirstName),

//                ("@ln", obj.LastName),

//                ("@dob", obj.DateOfBirth),

//                ("@ci", obj.CountryId),

//                ("@gi", obj.GenderId))) == 1;
    }
}