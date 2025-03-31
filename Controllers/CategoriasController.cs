using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrimeiraAPI.Context;
using PrimeiraAPI.Filters;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configurations;
        private readonly ILogger _logger;


        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _configurations = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpGet("LerArquivoConfiguracao")]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public string GetValores()
        {
            var valor1 = _configurations["chave1"];
            var valor2 = _configurations["chave2"];
            var secao1 = _configurations["secao1:chave2"];

            return $"Chave1 = {valor1} Chave2 = {valor2} Seção1 = {secao1}";
        }

        [HttpGet("produtos")]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("==========GET API==========");
            return _context.Categorias.Include(p=> p.Produtos).Where(x => x.CategoriaId <= 5).ToList();  
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            throw new ArgumentException("Ocorreu um erro no tratamento do request");

            /* var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound();
            }
            return Ok(categoria); */
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest();
            }

            _context.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound();
            }

            _context.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
