using PrimeiraAPI.Models;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPorCategoria(int id);        
    }
}
