using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGet.Protocol.Core.Types;
using PrimeiraAPI.Context;
using PrimeiraAPI.DTOs;
using PrimeiraAPI.DTOs.Mappings;
using PrimeiraAPI.Filters;
using PrimeiraAPI.Models;
using PrimeiraAPI.Pagination;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    public class CategoriasController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configurations;
        private readonly ILogger _logger;


        public CategoriasController(IRepository<Categoria> repository, IConfiguration configuration, ILogger<CategoriasController> logger, IUnitOfWork unitOfWork)
        {
            _configurations = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [DisableRateLimiting]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

            var categoriasDto = categorias.ToCategoriaDTOs();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o id = {id} não encontrada...");
                return NotFound($"Categoria com  o id = {id} não encontrada...");
            }

            var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);
        }
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inváldidos");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos..");
                return BadRequest("Dados inválidos");
            }

            var categoria = categoriaDTO.ToCategoria();

            var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();


            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
             var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);   

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o {id} não encontrada...");
                return NotFound();
            }

            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluida);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PageList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            var categoriasDto = categorias.ToCategoriaDTOs();

            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            return ObterCategorias(categorias);
        }

        [HttpGet("filter/pagination/nome")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetFilter([FromQuery] CategoriaFiltroNome categoriaFiltroNome)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasFilterNomesAsync(categoriaFiltroNome);
            return  ObterCategorias(categorias);
        }

    }
}
