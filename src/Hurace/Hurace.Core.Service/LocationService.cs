using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationDao _locationDao;

        public LocationService(ILocationDao locationDao)
        {
            _locationDao = locationDao;
        }

        public Task<IEnumerable<Location>> GetAllLocations() => _locationDao.FindAllAsync();

        public Task<IEnumerable<Discipline>> GetDisciplinesForLocation(int locationId) =>
            _locationDao.GetPossibleDisciplinesForLocation(locationId);
    }
}