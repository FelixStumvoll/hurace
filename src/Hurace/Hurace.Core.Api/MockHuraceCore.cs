using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Core.Dto;

namespace Hurace.Core.Api
{
    public class MockHuraceCore : IHuraceCore
    {
        public Task<ICollection<Gender>> GetGenders()
        {
            throw new System.NotImplementedException();
//            return Task.FromResult(new List<Gender> (){new Gender {Id = 1, GenderDescription = "Male"}});
        }

        public Task<ICollection<Location>> GetLocations()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<Discipline>> GetDisciplines()
        {
            await Task.Delay(100);
            return new List<Discipline>
            {
                new Discipline {DisciplineName = "Downhill"}, new Discipline {DisciplineName = "Super-G"}
            };
        }
    }
}