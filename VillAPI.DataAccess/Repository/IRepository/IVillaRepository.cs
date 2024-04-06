using System.Linq.Expressions;
using VillaAPI.Models;

namespace VillAPI.DataAccess.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);

    }
}
