using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Common;
using Hurace.Core.Dal.Dao;
using Hurace.Core.Dto;
using Hurace.Dal.Interface;

namespace Hurace.DataGenerator
{
    public class DbDataCreator
    {
        private string _providerName;
        private string _connectionString;
        private ConcreteConnectionFactory _connectionFactory;
        private IEnumerable<Country> _countries;
        private IEnumerable<Location> _locations;
        private ICountryDao _countryDao;
        private ILocationDao _locationDao;
        
        
        public DbDataCreator(string providerName, string connectionString)
        {
            _providerName = providerName;
            _connectionString = connectionString;
            _connectionFactory = new ConcreteConnectionFactory(DbUtil.GetProviderFactory(providerName), connectionString, providerName);
//            _countryDao = new CountryDao(_connectionFactory);
//            _locationDao = new LocationDao(_connectionFactory);
        }

        private async Task LoadFixedData()
        {
            _countries = await _countryDao.FindAllAsync();
            _locations = await _locationDao.FindAllAsync();
        }

        private void GenerateSkier(int gender, int amount)
        {
            
        }
        
        private void Run()
        {

        }
    }
}