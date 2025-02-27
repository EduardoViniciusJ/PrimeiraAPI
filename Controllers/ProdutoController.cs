using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Context;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.Controllers
{
    [Route("api/[controller]")]
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
            try
            {
                var produtos = _context.Produtos.AsNoTracking().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos nao encontrados");
                }
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status505HttpVersionNotsupported,"Ocorreu um problema");
            }
        }


        // Restrição rota exemplo
        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> Get2(string valor)
        {
            var teste = valor;

            return Ok(valor);
        }

        [HttpGet("/primeiro")] // ignora o routeamento padrão
        public ActionResult<Produto> GetPrimeiro(string valor)
        {
            var teste = valor;
            var produto = _context.Produtos.FirstOrDefault();
            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }



        // Acha o produto pelo seu Id 
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound();
            }
            return Ok(produto);
        }


        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest("Dados inválidos"); // Se os dados da requesicao forem invalidos retorna 400
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);  // cria um link para rota, retorna 201 do produto criado
        }


        // Atualiza todas as propriedades do produto pelo seu id 
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Dados inválidos");
            }

            _context.Entry(produto).State = EntityState.Modified; // Modifica pelo EF que o objeto foi alterado
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não localizado");  
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }







    }
}
