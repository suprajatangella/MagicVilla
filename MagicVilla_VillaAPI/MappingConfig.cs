using AutoMapper;
using VillaAPI.Models;
using VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Villa, VilaDTO>();
            CreateMap<VilaDTO, Villa>();

            CreateMap<Villa, VilaCreateDTO>().ReverseMap();
            CreateMap<Villa, VilaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
