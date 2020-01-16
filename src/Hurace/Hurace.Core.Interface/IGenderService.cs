using System.Collections.Generic;
using System.Threading.Tasks;
using Hurace.Dal.Domain;

namespace Hurace.Core.Interface
{
    public interface IGenderService
    {
        Task<IEnumerable<Gender>> GetAllGenders();
    }
}