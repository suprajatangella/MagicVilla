using AutoMapper;
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using VillaAPI.Models;
using VillAPI.DataAccess.Data;
using VillAPI.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VillAPI.DataAccess.Repository.IRepository;
using VillaAPI.Models.Dto;
using Azure;
using Newtonsoft.Json;
using Moq;
using Microsoft.AspNetCore.Http;

namespace MagicVillas.TestProject
{
    [TestFixture]

    public class SqlVillaControllerShould : Profile
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: "VillaDbControllerTest")
        .Options;

        ApplicationDbContext context;
        IVillaRepository villaRepository;
        VillaAPIController villaController;
        private IMapper _mapper;
        private MapperConfiguration configuration;

       
       

        [OneTimeSetUp]
        public void MappingConfiguration()
        {
            configuration = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<VilaDTO, VilaCreateDTO>().ReverseMap();
                //cfg.CreateMap<VilaDTO, VilaUpdateDTO>().ReverseMap();

                cfg.CreateMap<VilaDTO, Villa>().
                ForMember(d=>d.CreatedDate, s=>s.Ignore()).ForMember(d=>d.UpdatedDate, s=>s.Ignore())
                .ReverseMap();
            });
            _mapper = configuration.CreateMapper();
        }
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

            villaRepository = new VillaRepository(context);

            villaController = new VillaAPIController(villaRepository, _mapper);
            villaController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(),
            };
        }

        [Test, Order(1)]
        public void HTTPGET_GetAllVillas_WithFilterSearchStrPageNo_ReturnOk_Test()
        {
            var actionResult = villaController.GetVillas(4, "pool", 0, 1);

            // Extracting data from action result
            OkObjectResult okObjectResult = (actionResult.Result.Result) as OkObjectResult;
            APIResponse response = (okObjectResult.Value) as APIResponse;

            // Convert APIResponse to JSON string
            string jsonStr = JsonConvert.SerializeObject(response.Result);

            // Deserialize JSON string to DTO
            List<VilaDTO> model = JsonConvert.DeserializeObject<List<VilaDTO>>(jsonStr);

            // Map DTO to domain model
            List<Villa> villaList = _mapper.Map<List<Villa>>(model);

            Assert.That(villaList.First().Name, Does.Contain("pool").IgnoreCase);
            Assert.That(villaList.First().Id, Is.EqualTo(2));
            Assert.That(villaList.Count, Is.EqualTo(3));

            Assert.IsTrue(villaController.Response.Headers.ContainsKey("X-Pagination"));
        }

        [Test, Order(2)]
        public void HTTPGET_GetVillaById_ReturnsOk_Test()
        {
            int Id = 1;

            // Get villa information from controller
            var actionResult = villaController.GetVilla(Id);

            // Extracting data from action result
            OkObjectResult okObjectResult = (actionResult.Result.Result) as OkObjectResult;
            APIResponse response = (okObjectResult.Value) as APIResponse;

            // Convert APIResponse to JSON string
            string jsonStr = JsonConvert.SerializeObject(response.Result);

            // Deserialize JSON string to DTO
            VilaDTO model = JsonConvert.DeserializeObject<VilaDTO>(jsonStr);

            // Map DTO to domain model
            Villa villa = _mapper.Map<Villa>(model);

            // Asserting the properties of the villa
            Assert.That(villa.Id, Is.EqualTo(1));
            Assert.That(villa.Name, Is.EqualTo("royal villa").IgnoreCase);
        }


        [Test, Order(3)]
        public void HTTPGET_GetVillaById_ReturnsNotFound_Test()
        {
            int Id = 99;

            var actionResult = villaController.GetVilla(Id);

            NotFoundResult notFoundObjectResult = (actionResult.Result.Result) as NotFoundResult;

            Assert.That(notFoundObjectResult, Is.TypeOf<NotFoundResult>());

        }

        [Test, Order(4)]
        public void HTTPPOST_AddVilla_ReturnsCreated_Test()
        {
            var vilaCreateDTO = new VilaCreateDTO()
            {
                Name = "New Create Villa",
                Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                Occupancy = 3,
                Rate = "200",
                Sqft = 550,
                Amenity = ""
            };

            var actionResult = villaController.CreateVilla(vilaCreateDTO);

            CreatedResult createdObjectResult = (actionResult.Result.Result) as CreatedResult;

            Assert.That(createdObjectResult, Is.TypeOf<CreatedResult>());

        }

        [Test, Order(5)]
        public void HTTPDELETE_DeletePublisherById_ReturnsOk_Test()
        {
            int Id = 5;

            var actionResult = villaController.DeleteVilla(Id);

            OkResult deletedObjectResult = (actionResult.Result.Result) as OkResult;

            Assert.That(deletedObjectResult, Is.TypeOf<OkResult>());

        }

        [Test, Order(6)]
        public void HTTPDELETE_DeletePublisherById_ReturnsBadRequest_Test()
        {
            int Id = 6;

            var actionResult = villaController.DeleteVilla(Id);

            BadRequestObjectResult deletedObjectResult = (actionResult.Result.Result) as BadRequestObjectResult;

            Assert.That(deletedObjectResult, Is.TypeOf<BadRequestObjectResult>());
        }


    }
}