using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id);
        Task<PageList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<PageList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosParameters produtosParameters);


    }
}
