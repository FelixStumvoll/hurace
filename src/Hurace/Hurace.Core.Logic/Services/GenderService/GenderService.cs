using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.GenderService
{
    [ExcludeFromCodeCoverage]
    public class GenderService : IGenderService
    {
        private readonly IGenderDao _genderDao;
        
        
        public GenderService(IGenderDao genderDao)
        {
            _genderDao = genderDao;
        }

        public Task<IEnumerable<Gender>> GetAllGenders() => _genderDao.FindAllAsync();
    }
}