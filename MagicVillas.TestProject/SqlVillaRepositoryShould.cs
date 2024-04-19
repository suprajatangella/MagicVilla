using VillAPI.DataAccess.Data;
using VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Linq.Expressions;
using VillAPI.DataAccess.Repository;
using AutoMapper;
using MagicVilla_VillaAPI.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VillAPI.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVillas.TestProject
{

    [TestFixture]
    public class SqlVillaRepositoryShould : Profile
    {

        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
       .UseInMemoryDatabase(databaseName: "VillaDbRepositoryTest")
       .Options;

        ApplicationDbContext context;

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
        }


        [Test, Order(1)] 
        public async Task CreateVillaAsync()
        {
            //Arrange
            var sut = new VillaRepository(context);

            var villa = GetVillaData();
            //Act
            var result = sut.CreateAsync(villa);

            List<Villa> villaList = context.Villas.ToList();
            //Assert
            //villaList.Contains(villa);
            CollectionAssert.Contains(villaList, villa);
        }

        [Test, Order(2)]
        public async Task GetVillaAsync()
        {
            //Arrange
            int Id = 1;

            var sat = new VillaRepository(context);

            //Act

            Villa villa = await sat.GetAsync(v=>v.Id == Id);

            //Assert

            Assert.NotNull(villa);

            Assert.IsTrue(villa.Id == Id);

        }

        [Test, Order(3)]
        public async Task GetVillasAsync()
        {
            //Arrange

            var vList = context.Villas.ToList();

            var sat = new VillaRepository(context);

            //Act

            List<Villa> villaList = await sat.GetAllAsync(v => v.Occupancy == 4);

            //Assert

            Assert.NotNull(villaList);

            Assert.That(villaList.Count, Is.EqualTo(vList.FindAll(v=>v.Occupancy==4).Count));

        }

        [Test, Order(4)]
        public async Task UpdateVillaAsync()
        {

            //Arrange
            List<Villa> villaList = context.Villas.ToList();

            int Id = 2;

            Villa villa = villaList.FirstOrDefault(v=>v.Id == Id);

            villa.Name = "Test Project Update Villa";

            //Act

            context.Villas.Update(villa);

            await context.SaveChangesAsync();

            var sat = new VillaRepository(context);

            Villa updatedVilla = await sat.GetAsync(v => v.Id == Id);

            //Assert

            Assert.That(villa.Name, Is.EqualTo(updatedVilla.Name));

        }

        [Test, Order(5)]
        public async Task RemoveVillaAsync()
        {
            //Arrange
            List<Villa> villaList = context.Villas.ToList();

            int Id = 3;

            Villa deleteVilla = villaList.FirstOrDefault(v => v.Id == Id);

            //Act

            context.Villas.Remove(deleteVilla);

            await context.SaveChangesAsync();

            var sat = new VillaRepository(context);

            Villa chkDeletedVilla = await sat.GetAsync(v => v.Id == Id);

            //Assert

            Assert.Null(chkDeletedVilla);

        }

        // The GetVillaData method is provided below:

        private static Villa GetVillaData()
        {
            Villa villa = new Villa
            {
                //Id = 1,
                Name = "Royal Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa5.jpg",
                Occupancy = 4,
                Rate = "200",
                Sqft = 550,
                Amenity = "",
                CreatedDate = DateTime.Now
            };
            return villa;
        }

    }
}