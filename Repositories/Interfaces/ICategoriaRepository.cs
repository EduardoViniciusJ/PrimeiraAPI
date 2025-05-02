using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PageList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);

        PageList<Categoria> GetCategoriasFilterNomes(CategoriaFiltroNome categoriasParameters);

    }
}
