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

        public PageList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var  categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();   

            var categoriasOrdenadas = PageList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasOrdenadas;
        }

        public PageList<Categoria> GetCategoriasFilterNomes(CategoriaFiltroNome categoriasParameters)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
            }

            var categoriaFiltradas = PageList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriaFiltradas;

        }
    }
}
