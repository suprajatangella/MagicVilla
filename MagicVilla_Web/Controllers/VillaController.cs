﻿using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
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
            _villaService= villaService;

            _mapper= mapper;
        }
        public async Task<ActionResult> CreateVilla()
        {
            return View();
        }
        public async Task<ActionResult> IndexVilla()
        {
            List<VilaDTO> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<VilaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
