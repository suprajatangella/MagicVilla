using VillAPI.DataAccess.Data;
using VillaAPI.Models;
using VillAPI.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace VillAPI.DataAccess.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context;


        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<VillaNumber> UpdateAsync(VillaNumber villaNumber)
        {
            villaNumber.UpdatedDate = DateTime.Now;
            _context.Entry(villaNumber).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return villaNumber;
        }
    }
}
