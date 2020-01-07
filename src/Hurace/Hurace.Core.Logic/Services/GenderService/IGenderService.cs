using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Logic.Services.GenderService
{
    public interface IGenderService
    {
        Task<IEnumerable<Gender>> GetAllGenders();
    }
}