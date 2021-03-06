﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Hurace.Core.Interface;
using Hurace.Core.Service.Util;
using Hurace.Dal.Domain;
using Hurace.Dal.Interface;

namespace Hurace.Core.Service
{
    public class RaceStartListService : IRaceStartListService
    {
        private readonly IStartListDao _startListDao;
        private readonly ISkierDao _skierDao;

        public RaceStartListService(IStartListDao startListDao, ISkierDao skierDao)
        {
            _startListDao = startListDao;
            _skierDao = skierDao;
        }

        public async Task<bool> UpdateStartList(int raceId, IEnumerable<StartList> startList)
        {
            using var scope = ScopeBuilder.BuildTransactionScope();
            await _startListDao.DeleteAllForRace(raceId);
            foreach (var sl in startList) await _startListDao.InsertAsync(sl);
            scope.Complete();
            return true;
        }

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<Skier>> GetAvailableSkiersForRace(int raceId) =>
            _skierDao.FindAvailableSkiersForRace(raceId);

        [ExcludeFromCodeCoverage]
        public Task<IEnumerable<StartList>> GetStartListForRace(int raceId) =>
            _startListDao.GetStartListForRace(raceId);
        
        public async Task<bool?> IsStartListDefined(int raceId) =>
            (await _startListDao.CountStartListForRace(raceId) ?? 0) != 0;
        
        [ExcludeFromCodeCoverage]
        public Task<StartList?> GetStartListById(int skierId, int raceId) =>
            _startListDao.FindByIdAsync(skierId, raceId);
    }
}