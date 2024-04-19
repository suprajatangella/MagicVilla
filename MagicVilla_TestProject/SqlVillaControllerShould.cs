using AutoMapper;
using MagicVilla_VillaAPI.Controllers.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using VillaAPI.Models;
using VillAPI.DataAccess.Data;
using VillAPI.DataAccess.Repository;
using AutoMapper;
using MagicVilla_VillaAPI;
using VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Azure;

namespace MagicVilla_TestProject
{
    [TestFixture]
    public class SqlVillaControllerShould : Profile
    {
        private readonly ApplicationDbContext _context;

        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
         .UseInMemoryDatabase(databaseName: "VillaDbControllerTest")
         .Options;

        ApplicationDbContext context;
        VillaRepository villaRepository;
        VillaAPIController villaController;
        private IMapper _mapper;

        [SetUp]
        public MapperConfiguration MappingConfigure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingConfig>();
            });
            return config;
        }

       [SetUp]
        public void Setup()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();

            villaRepository = new VillaRepository(context);

            var config = MappingConfigure();
            _mapper = config.CreateMapper();

            villaController = new VillaAPIController(villaRepository, _mapper);
        }

        [Fact]
        public void HTTPGET_GetAllVillas_WithFilterSearchStrPageNo_ReturnOk_Test()
        {
            Task<ActionResult<APIResponse>> actionResult = villaController.GetVillas(4,"pool",0,1);

            NUnit.Framework.Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            List<Villa> villaList = _mapper.Map<List<Villa>>(actionResult.Result);

           
            NUnit.Framework.Assert.That(villaList.First().Name, Does.Contain("pool").IgnoreCase);
            NUnit.Framework.Assert.That(villaList.First().Id, Is.EqualTo(2));
            NUnit.Framework.Assert.That(villaList.Count, Is.EqualTo(1));
        }

        private void SeedDatabase()
        {
            var villaList = new List<Villa>
            { new Villa
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

       
            context.Villas.AddRange(villaList);

            context.SaveChanges();
        }
    }
}
