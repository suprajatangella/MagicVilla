using VillAPI.DataAccess.Data;
using VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Linq.Expressions;
using VillAPI.DataAccess.Repository;

namespace MagicVilla_TestProject
{
    public class SqlVillaRepositoryShould
    {

        private readonly ApplicationDbContext _context;
        public SqlVillaRepositoryShould()
        {
            //Microsoft.EntityFrameworkCore.InMemory
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseInMemoryDatabase(
                        databaseName: Guid.NewGuid().ToString()
                );
            _context = new ApplicationDbContext(optionsBuilder.Options);
        }

        [Fact]
        public async Task CreateVillaAsync()
        {
            //Arrange
            var sut = new VillaRepository(_context);

            var villa = GetVillaData();
            //Act
            var result = sut.CreateAsync(villa);

            List<Villa> villaList = _context.Villas.ToList();
            //Assert
            Assert.Single(villaList);
        }

        [Fact]
        public async Task GetVillaAsync()
        {
            //Arrange
            int Id = 1;
            _context.Villas.Add(GetVillaData());

            await _context.SaveChangesAsync();

            var sat = new VillaRepository( _context );

            //Act

            Villa villa = await sat.GetAsync(v=>v.Id == Id);

            //Assert

            Assert.NotNull(villa);

        }

        [Fact]
        public async Task GetVillasAsync()
        {
            //Arrange

            var vList = GetVillaListData();

            _context.Villas.AddRange(vList);

            await _context.SaveChangesAsync();

            var sat = new VillaRepository(_context);

            //Act

            List<Villa> villaList = await sat.GetAllAsync(v => v.Occupancy == 4);

            //Assert

            Assert.NotNull(villaList);
            Assert.Equal(villaList.Count, vList.FindAll(v=> v.Occupancy==4).Count);

        }

        [Fact]
        public async Task UpdateVillaAsync()
        {

            //Arrange
            List<Villa> villaList = GetVillaListData().ToList();

            _context.Villas.AddRange(villaList);
            await _context.SaveChangesAsync();

            int Id = 2;

            Villa villa = villaList.FirstOrDefault(v=>v.Id == Id);

            villa.Name = "Test Project Update Villa";

            //Act

            _context.Villas.Update(villa);

            await _context.SaveChangesAsync();

            var sat = new VillaRepository(_context);

            Villa updatedVilla = await sat.GetAsync(v => v.Id == Id);

            //Assert

            Assert.Equal(villa.Name, updatedVilla.Name);

        }

        [Fact]
        public async Task RemoveVillaAsync()
        {
            //Arrange
            List<Villa> villaList = GetVillaListData().ToList();

            _context.Villas.AddRange(villaList);
            await _context.SaveChangesAsync();

            int Id = 3;

            Villa deleteVilla = villaList.FirstOrDefault(v => v.Id == Id);

            //Act

            _context.Villas.Remove(deleteVilla);

            await _context.SaveChangesAsync();

            var sat = new VillaRepository(_context);

            Villa chkDeletedVilla = await sat.GetAsync(v => v.Id == Id);

            //Assert

            Assert.Null(chkDeletedVilla);

        }

        // The GetVillaData method is provided below:

        private static Villa GetVillaData()
        {
            Villa villa = new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                Occupancy = 4,
                Rate = "200",
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            };
            return villa;
        }

        // The GetVillaListData method is provided below:

        private static List<Villa> GetVillaListData()
        {
            List<Villa> villaList = new List<Villa>() { new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                Occupancy = 4,
                Rate = "200",
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            },
              new Villa
              {
                  Id = 2,
                  Name = "Premium Pool Villa",
                  Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                  ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                  Occupancy = 4,
                  Rate = "300",
                  Sqft = 550,
                  Amenity = "",
                  CreatedDate = DateTime.Now
              },
             new Villa
             {
                 Id = 3,
                 Name = "Luxury Pool Villa",
                 Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                 ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                 Occupancy = 3,
                 Rate = "400",
                 Sqft = 750,
                 Amenity = "",
                 CreatedDate = DateTime.Now
             } };

            return villaList;
        }
    }
}