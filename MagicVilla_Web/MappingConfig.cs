using AutoMapper;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {

        public MappingConfig() {
            CreateMap<VilaDTO, VilaCreateDTO>().ReverseMap();
            CreateMap<VilaDTO, VilaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

        }
        
    }
}
