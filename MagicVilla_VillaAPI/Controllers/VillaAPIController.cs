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
using System.Net;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers
{

    [ApiController]

    [Route("api/VillaAPI")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        private readonly IVillaRepository _db;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaAPIController(ILogging logger, IVillaRepository db, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _db.GetAllAsync();
                _logger.Log("Getting All Villas", "Info");
                _response.Result = _mapper.Map<List<VilaDTO>>(villaList);
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

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.Log("Error while retrieving Villa with Id" + id, "error");
                    return BadRequest();
                }

                var villa = await _db.GetAsync(v => v.Id == id);

                if (villa == null)
                {
                    _logger.Log("Villa details does not exists with Id" + id, "error");
                    return NotFound();
                }

                _response.Result = _mapper.Map<VilaDTO>(villa);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VilaCreateDTO createDTO)
        {
            try
            {
                if (await _db.GetAsync(v => v.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Villa villa = _mapper.Map<Villa>(createDTO);


                await _db.CreateAsync(villa);

                _response.Result = _mapper.Map<VilaCreateDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                { return BadRequest(); }


                var villa = await _db.GetAsync(v => v.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                await _db.RemoveAsync(villa);
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VilaUpdateDTO updateDTO)
        {
            try

            {
                if (updateDTO == null || id != updateDTO.Id)
                { return BadRequest(); }

                Villa model = _mapper.Map<Villa>(updateDTO);

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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VilaUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                { return BadRequest(); }

                var villa = await _db.GetAsync(v => v.Id == id, false);

                if (villa == null)
                {
                    return NotFound();
                }

                VilaUpdateDTO vilaDTO = _mapper.Map<VilaUpdateDTO>(villa);

                patchDTO.ApplyTo(vilaDTO, ModelState);

                Villa model = _mapper.Map<Villa>(vilaDTO);

                await _db.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                if (!ModelState.IsValid)
                { return BadRequest(); }

                return NoContent();
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
