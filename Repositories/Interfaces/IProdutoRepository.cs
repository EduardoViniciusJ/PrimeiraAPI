using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPorCategoria(int id);

        IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
    }
}
