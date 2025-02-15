using Microsoft.AspNetCore.Mvc;
using PrimeiraAPI.Context;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        // Pega a lista de todos os produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound("Produtos nao encontrados");
            }
            return produtos;
        }

        // Acha o produto pelo seu Id 
        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound();
            }
            return produto; 
        }










    }
}
