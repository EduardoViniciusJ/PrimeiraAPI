using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PageList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
        Task<PageList<Categoria>> GetCategoriasFilterNomesAsync(CategoriaFiltroNome categoriasParameters);

    }
}
