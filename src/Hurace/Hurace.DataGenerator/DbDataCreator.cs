using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dal.Dao.QueryBuilder;
using Hurace.Core.Dal.Dao.QueryBuilder.ConcreteQueryBuilder;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;
using Newtonsoft.Json;

namespace Hurace.DataGenerator
{
    public class DbDataCreator
    {
        private string _providerName;
        private string _connectionString;
        private ConcreteConnectionFactory _connectionFactory;
        private IEnumerable<Country> _countries;
        private IEnumerable<Location> _locations;
        private readonly ICountryDao _countryDao;
        private readonly ILocationDao _locationDao;
        private QueryFactory _queryFactory;

        public DbDataCreator(string providerName, string connectionString)
        {
            _providerName = providerName;
            _connectionString = connectionString;
            _connectionFactory =
                new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providerName), connectionString, providerName);
            _queryFactory = new QueryFactory("hurace");
            _countryDao = new CountryDao(_connectionFactory, _queryFactory);
            _locationDao = new LocationDao(_connectionFactory, _queryFactory);
        }

        private async Task LoadFixedData()
        {
            _countries = await _countryDao.FindAllAsync();
            _locations = await _locationDao.FindAllAsync();
        }

        private DateTime GetRandomBirthDate()
        {
            var baseDate = new DateTime(1985, 1, 1);
            var rand = new Random();
            return baseDate.AddDays(rand.Next(-300, 300));
        }

        private int GetCountryId(string country) =>
            _countries.FirstOrDefault(c => c.CountryName.Equals(country))?.Id ?? -1;

        private int GetGenderId(string gender) =>
            gender switch
            {
                "f" => 2,
                "m" => 1,
                _ => -1,
            };

        private (string firstname, string lastname) GetName(string name)
        {
            var spaceIndex = name.IndexOf(" ", StringComparison.Ordinal);
            return (name.Substring(0, spaceIndex), name.Substring(spaceIndex+1));
        }

        private void GenerateSkier()
        {
            using var streamReader = new StreamReader("Data.json");
            var json = streamReader.ReadToEnd();
            var skier = JsonConvert.DeserializeObject<List<JsonData>>(json);

            var skierObj = skier.Select(jdata =>
            {
                var (firstname, lastname) = GetName(jdata.Name);
                return new Skier
                {
                    CountryId = GetCountryId(jdata.Country),
                    GenderId = GetGenderId(jdata.Gender),
                    FirstName = firstname,
                    LastName = lastname,
                    DateOfBirth = GetRandomBirthDate()
                };
            }).ToList();
        }

        public async Task Run()
        {
            await LoadFixedData();
            GenerateSkier();
        }
    }
}