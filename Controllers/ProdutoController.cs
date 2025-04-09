using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Context;
using PrimeiraAPI.Models;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProdutoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("produtos/ {id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategorias(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutoPorCategoria(id);
            if (produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }


        // Pega a lista de todos os produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _unitOfWork.ProdutoRepository.GetAll();
            if (produtos is null)
            {
                return NotFound();
            }

            return Ok(produtos);
       
        }

        // Acha o produto pelo seu Id 
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);  

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            return Ok(produto);
        }


        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }

            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, produto);


        }


        // Atualiza todas as propriedades do produto pelo seu id 
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado");  
            }
            
            _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();
            return Ok(produto);

        }


    }

}
