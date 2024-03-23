using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Models.Repository.IRepository;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{

    [ApiController]

    [Route("api/VillaNumberAPI")]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly IVillaNumberRepository _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaNumberAPIController(ILogging logger, IVillaNumberRepository db, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _db.GetAllAsync(includeProperties:"Villa");
                _logger.Log("Getting All Villa Numbers", "Info");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Error while retrieving Villa Number with Id" + id, "error");
                    return BadRequest();
                }

                var villaNumber = await _db.GetAsync(v => v.VillaNo == id);

                if (villaNumber == null)
                {
                    _logger.Log("Villa Number details does not exists with Id" + id, "error");
                    return NotFound();
                }

                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                if (await _db.GetAsync(v => v.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number Already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);


                await _db.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<List<VillaNumberCreateDTO>>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                { return BadRequest(); }


                var villaNumber = await _db.GetAsync(v => v.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                await _db.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try

            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                { return BadRequest(); }

                var villaNumber = await _db.GetAsync(v => v.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumber);

                await _db.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "UpdateNumberPartialVilla")]
        public async Task<ActionResult<APIResponse>> UpdateNumberPartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null) //|| id != patchDTO.Id)
                { return BadRequest(); }

                var villaNumber = await _db.GetAsync(v => v.VillaNo  == id, false);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                VillaNumberUpdateDTO villaNumberDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

                patchDTO.ApplyTo(villaNumberDTO, ModelState);

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDTO);

                await _db.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                if (!ModelState.IsValid)
                { return BadRequest(); }

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }
    }
}
