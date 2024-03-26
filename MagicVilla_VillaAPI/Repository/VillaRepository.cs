﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Villa> UpdateAsync(Villa villa)
        {

            villa.UpdatedDate = DateTime.Now;
            _context.Villas.Update(villa);
            await _context.SaveChangesAsync();
            return villa;
        }


    }
}
