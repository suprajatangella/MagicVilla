using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{

    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;

        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;

            _mapper = mapper;
        }
        public async Task<ActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVilla(VilaCreateDTO createDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(createDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Created Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Error Encountered.";
            return View(createDTO);
        }


        public async Task<ActionResult> UpdateVilla(int id)
        {
            var response = await _villaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {

                VilaDTO model = JsonConvert.DeserializeObject<VilaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VilaUpdateDTO>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateVilla(VilaUpdateDTO updateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(updateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            TempData["error"] = "Error Encountered.";
            return View(updateDTO);
        }
        public async Task<ActionResult> DeleteVilla(int id)
        {
            var response = await _villaService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {

                VilaDTO model = JsonConvert.DeserializeObject<VilaDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteVilla(VilaDTO model)
        {

            var response = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                    TempData["success"] = "Villa deleted Successfully";
                    return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Error Encountered.";
            return View(model);
        }

        public async Task<ActionResult> IndexVilla()
        {
            List<VilaDTO> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VilaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
