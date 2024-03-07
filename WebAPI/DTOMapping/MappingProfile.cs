using AutoMapper;
using WebAPI.Models;
using WebAPI.Models.DTOs;

namespace WebAPI.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalPark,NationalParkDto>().ReverseMap();
            CreateMap<TrailsDto,Trails>().ReverseMap();
        }
    }
}
