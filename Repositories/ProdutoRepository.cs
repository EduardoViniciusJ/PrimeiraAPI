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

        public IEnumerable<Produto> GetProdutoPorCategoria(int id)
        {
            return _context.Produtos.Where(p => p.CategoriaId == id).ToList();
        }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return GetAll()
                .OrderBy(p => p.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList();
        }

        public PageList<Produto> GetProdutosFiltroPreco(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().AsQueryable();

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
            var produtosFiltrados = PageList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosFiltrados;
        }

        PageList<Produto> IProdutoRepository.GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();

            var produtosOrdernandos = PageList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdernandos;
        }
    }
}
