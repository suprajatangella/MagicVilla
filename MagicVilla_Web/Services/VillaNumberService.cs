﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json.Linq;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory) 
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaNumberAPI",

                ApiType = SD.ApiType.POST,

                Data = dto,

                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaNumberAPI/" + id,

                ApiType = SD.ApiType.DELETE,

                Token = token

            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/villaNumberAPI/",

                ApiType = SD.ApiType.GET,

                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaNumberAPI/" + id,

                ApiType = SD.ApiType.GET,

                Token = token

            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaNumberAPI/" + dto.VillaNo,

                ApiType = SD.ApiType.PUT,

                Data = dto,

                Token = token
            });
        }
    }
}
