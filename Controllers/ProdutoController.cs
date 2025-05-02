using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Context;
using PrimeiraAPI.DTOs;
using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("produtos/ {id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategorias(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutoPorCategoria(id);
            if (produtos is null)
            {
                return NotFound();
            }

            var produtosDto = _mapper.Map<ProdutoDTO>(produtos);

            return Ok(produtosDto);
        }


        // Pega a lista de todos os produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _unitOfWork.ProdutoRepository.GetAll();
            if (produtos is null)
            {
                return NotFound();
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);

        }

        // Acha o produto pelo seu Id 
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDto);
        }


        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {
            if (produtoDTO is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO); // Mapeia o DTO para o modelo Produto   
            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto); // Mapeia o novo produto para o DTO

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);


        }


        // Atualiza todas as propriedades do produto pelo seu id 
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO); // Mapeia o DTO para o modelo Produto
            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado); // Mapeia o produto atualizado para o DTO

            return Ok(produtoAtualizadoDto);
        }

        [HttpPatch("{id:int}")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateResquest> patchProdutoDTO)
        {
            if (patchProdutoDTO is null)
            {
                return BadRequest();
            }

            var produto = _unitOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoUpdateResquest = _mapper.Map<ProdutoDTOUpdateResquest>(produto); // Mapeia o produto para o DTO
            patchProdutoDTO.ApplyTo(produtoUpdateResquest, ModelState);

            _mapper.Map(produtoUpdateResquest, produto); // Mapeia o DTO para o produto 
            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto)); // Mapeia o produto atualizado para o DTO de resposta

        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDeltado = _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeltado); // Mapeia o produto para o DTO

            return Ok(produtoDeletadoDto);

        }

        private ActionResult<ProdutoDTO> ObterProdutos(PageList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalPages,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }


        [HttpGet("pagination")]
        public ActionResult<ProdutoDTO> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
            return ObterProdutos(produtos);
        }
        [HttpGet("filter/pagination/preco")]
        public ActionResult<ProdutoDTO> GetProdutosFilterPreco([FromQuery] ProdutosParameters produtosParameters)
        {

            var produtos = _unitOfWork.ProdutoRepository.GetProdutosFiltroPreco(produtosParameters);
            return ObterProdutos(produtos);
                
        }









    }

}
