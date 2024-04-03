using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;
        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory) 
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> CreateAsync<T>(VilaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/v1/VillaAPI",

                ApiType = SD.ApiType.POST,

                Data = dto,

                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/v1/VillaAPI/" + id,

                ApiType = SD.ApiType.DELETE,

                Token = token

            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/v1/villaAPI/",

                ApiType = SD.ApiType.GET,

                Token = token

            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/v1/VillaAPI/" + id,

                ApiType = SD.ApiType.GET,

                Token = token

            });
        }

        public Task<T> UpdateAsync<T>(VilaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/v1/VillaAPI/" + dto.Id,

                ApiType = SD.ApiType.PUT,

                Data = dto,

                Token = token
            });
        }
    }
}
