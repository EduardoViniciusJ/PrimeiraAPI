using PrimeiraAPI.Models;

namespace PrimeiraAPI.Repositories
{
    public interface IProdutoReposity
    {
        IQueryable<Produto>GetProdutos();
        Produto GetProduto(int id);
        Produto Create(Produto produto);
        bool Update(Produto produto);    
        bool Delete(int id);    

    }
}
