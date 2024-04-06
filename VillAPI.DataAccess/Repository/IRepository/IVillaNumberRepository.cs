using VillaAPI.Models;

namespace VillAPI.DataAccess.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber villa);
    }
}
