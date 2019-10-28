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
            connectionFactory, "hurace.skier")
        {
        }

        public override Task<IEnumerable<Skier>> FindAllAsync() =>
            QueryAsync<Skier>(
                @"select s.id, s.firstName,
                                        s.lastName,
                                        s.genderId,
                                        s.dateOfBirth,
                                        s.countryId,
                                        c.countryName,
                                        c.countryCode,
                                        g.genderDescription
                                        from hurace.Skier as s
                                    join hurace.Country as c on c.id = s.countryId
                                    join hurace.Gender as g on g.id = s.genderId", 
                new MapperConfig()
                    .AddMapping<Gender>(("genderId", "id"))
                    .AddMapping<Country>(("countryId", "id"), ("countryName", "name")));

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