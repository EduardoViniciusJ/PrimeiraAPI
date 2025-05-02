using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPorCategoria(int id);

        PageList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        PageList<Produto> GetProdutosFiltroPreco(ProdutosParameters produtosParameters);


    }
}
