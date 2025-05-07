 using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Context;
using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Repositories
{
    public class ProdutoRepository : Respository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();
            var produtosCategoria = produtos.Where(p => p.CategoriaId == id);

            return produtosCategoria;
        }

        public async Task<PageList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.Nome).AsQueryable();

            var resultado = PageList<Produto>.ToPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);   
           
            return resultado;
        }

        public async Task<PageList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();

            if (produtosParameters.Preco.HasValue && !string.IsNullOrEmpty(produtosParameters.PrecoCriterio))
            {
                if (produtosParameters.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosParameters.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosParameters.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosParameters.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosParameters.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosParameters.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            var produtosFiltrados = PageList<Produto>.ToPagedList(produtos.AsQueryable(), produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosFiltrados;
        }
    }
}
