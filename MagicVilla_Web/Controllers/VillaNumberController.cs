using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly IVillaNumberService _villaNumberService;


        private readonly IVillaService _villaService;

        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<ActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<ActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VilaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); ;
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
                List<VillaNumberDTO> list = new();
                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = "Villa Number created Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }


        public async Task<ActionResult> UpdateVillaNumber(int id)
        {
            VillaNumberUpdateVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VilaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }


            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateVillaNumber(VillaNumberUpdateVM updateDTO)
        {
            if (ModelState.IsValid)
            {

                var response = await _villaNumberService.UpdateAsync<APIResponse>(updateDTO.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Number updated Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                updateDTO.VillaList = JsonConvert.DeserializeObject<List<VilaDTO>>
                    (Convert.ToString(resp.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }); ;
            }
            TempData["error"] = "Error encountered.";
            return View(updateDTO);
        }
        public async Task<ActionResult> DeleteVillaNumber(int id)
        {
            VillaNumberDeleteVM villaNumberVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VilaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(villaNumberVM);
            }


            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Number deleted Successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            TempData["error"] = "Error encountered.";
            return View(model);
        }
    }
}
