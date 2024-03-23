using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Models.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);
        
    }
}
