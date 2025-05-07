using PrimeiraAPI.Context;
using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Repositories
{
    public class CategoriaRepository : Respository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PageList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriaOrdenadas = categorias.OrderBy(x => x.CategoriaId).AsQueryable();

            var resultado = PageList<Categoria>.ToPagedList(categoriaOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<PageList<Categoria>> GetCategoriasFilterNomesAsync(CategoriaFiltroNome categoriasParameters)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
            }

            var categoriaFiltradas = PageList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriaFiltradas;

        }
    }
}
