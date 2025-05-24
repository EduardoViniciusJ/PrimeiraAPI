using AutoMapper;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.DTOs.Mappings
{
    public class AuthDTOMappingProfile : Profile
    {
        public AuthDTOMappingProfile()
        {
            CreateMap<ApplicationUser, TokenModelDTO>().ReverseMap();
            CreateMap<ApplicationUser, LoginModelDTO>().ReverseMap();
            CreateMap<ApplicationUser, RegisterModelDTO>().ReverseMap();    
        }
    }
}
