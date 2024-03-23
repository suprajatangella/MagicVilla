﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Repository.IRepository;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Models.Repository
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
            _context.VillaNumbers.Update(villaNumber);
            await _context.SaveChangesAsync();
            return villaNumber;
        }
    }
}
