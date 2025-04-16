using PrimeiraAPI.Models;

namespace PrimeiraAPI.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria? categoria)
        {
            if (categoria is null)
            {
                return null;
            }

            return  new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImageUrl = categoria.ImageUrl
            };
        }   

        public static Categoria? ToCategoria(this CategoriaDTO? categoriaDTO)
        {
            if (categoriaDTO is null)
            {
                return null;
            }

            return new Categoria
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImageUrl = categoriaDTO.ImageUrl
            };
        }

        public static  IEnumerable<CategoriaDTO> ToCategoriaDTOs(this IEnumerable<Categoria> categorias)
        {
            if(categorias is null || !categorias.Any())
            {
                return Enumerable.Empty<CategoriaDTO>();
            }

            return categorias.Select(c => new CategoriaDTO
            {
                CategoriaId = c.CategoriaId,
                Nome = c.Nome,
                ImageUrl = c.ImageUrl
            });
        }      
    }
}
