using VillAPI.DataAccess.Data;
using VillaAPI.Models;
using VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using VillAPI.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers.v2
{

    [ApiController]
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        public VillaNumberAPIController()
        {
            
        }

        [HttpGet("GetString")]
        //[MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Supraja", "Tangella" };
        }

    }
}
