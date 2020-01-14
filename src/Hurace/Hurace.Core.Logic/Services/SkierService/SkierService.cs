using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Hurace.Core.Logic.Models;
using Hurace.Core.Logic.Util;
using Hurace.Dal.Dao;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Logic.Services.SkierService
{
    public class SkierService : ISkierService
    {
        private readonly ISkierDao _skierDao;
        private readonly ICountryDao _countryDao;
        private readonly IGenderDao _genderDao;

        public SkierService(ISkierDao skierDao, ICountryDao countryDao, IGenderDao genderDao)
        {
            _skierDao = skierDao;
            _countryDao = countryDao;
            _genderDao = genderDao;
        }

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Skier>> GetAllSkiers() => _skierDao.FindAllAsync();

        [ExcludeFromCodeCoverage]
        public Task<Skier?> GetSkierById(int id) => _skierDao.FindByIdAsync(id);
        
        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Discipline>> GetDisciplinesForSkier(int id) => 
            _skierDao.GetPossibleDisciplinesForSkier(id);

        private async Task<bool> SkierValidator(Skier skier) =>
            await _countryDao.FindByIdAsync(skier.CountryId) != null &&
            await _genderDao.FindByIdAsync(skier.GenderId) != null;

        [ExcludeFromCodeCoverage]
        public async Task<bool> UpdateSkier(Skier skier) => await SkierValidator(skier) && await _skierDao.UpdateAsync(skier);

        [ExcludeFromCodeCoverage]
        public async Task<int?> CreateSkier(Skier skier) => await SkierValidator(skier) ? await _skierDao.InsertGetIdAsync(skier) : null;

        public async Task<bool> UpdatePossibleDisciplines(int skierId, IEnumerable<int> disciplines)
        {
            var skier = await GetSkierById(skierId);
            if (skier == null) return false;
            using var scope = ScopeBuilder.BuildTransactionScope();
            await _skierDao.DeleteAllPossibleDisciplineForSkier(skierId);
            foreach (var discipline in disciplines)
                await _skierDao.InsertPossibleDisciplineForSkier(skierId, discipline);
            scope.Complete();
            return true;
        }
    }
}