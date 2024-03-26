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
        public Task<T> CreateAsync<T>(VilaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaAPI",

                ApiType = SD.ApiType.POST,

                Data = dto
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaAPI/" + id,

                ApiType = SD.ApiType.DELETE

            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/villaAPI/",

                ApiType = SD.ApiType.GET

            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaAPI/" + id,

                ApiType = SD.ApiType.GET

            });
        }

        public Task<T> UpdateAsync<T>(VilaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                Url = villaUrl + "/api/VillaAPI/" + dto.Id,

                ApiType = SD.ApiType.PUT,

                Data = dto
            });
        }
    }
}
