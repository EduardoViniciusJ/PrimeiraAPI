using AutoMapper;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.DTOs.Mappings
{
    public class ProdutoDTMappingProfile : Profile 
    {
        public ProdutoDTMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            
        }

    }
}
