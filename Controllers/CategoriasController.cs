using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NuGet.Protocol.Core.Types;
using PrimeiraAPI.Context;
using PrimeiraAPI.DTOs;
using PrimeiraAPI.Filters;
using PrimeiraAPI.Models;
using PrimeiraAPI.Repositories;
using PrimeiraAPI.Repositories.Interfaces;

namespace PrimeiraAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _unitOfWork.CategoriaRepository.GetAll();

            var categoriasDto = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDto = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImageUrl = categoria.ImageUrl
                };
                categoriasDto.Add(categoriaDto);
            }
            
            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com o id = {id} não encontrada...");
                return NotFound($"Categoria com  o id = {id} não encontrada...");
            }

            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImageUrl = categoria.ImageUrl
            };

            return Ok(categoriaDto);   
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inváldidos");
            }

            var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImageUrl = categoriaDto.ImageUrl
            };

             var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            _unitOfWork.Commit();

            var novacategoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaCriada.CategoriaId,
                Nome = categoriaCriada.Nome,
                ImageUrl = categoriaCriada.ImageUrl
            };
             
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos..");
                return BadRequest("Dados inválidos");
            }

            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImageUrl = categoriaDTO.ImageUrl
            };

            var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();


            var categoriaAtualizadaDto = new Categoria()
            {
                CategoriaId = categoriaAtualizada.CategoriaId,
                Nome = categoriaAtualizada.Nome,
                ImageUrl = categoriaAtualizada.ImageUrl
            };

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com o {id} não encontrada...");
                return NotFound();
            }

            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();

            var categoriaExcluidaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaExcluida.CategoriaId,
                Nome = categoriaExcluida.Nome,
                ImageUrl = categoriaExcluida.ImageUrl
            };

            return Ok(categoriaExcluidaDto);
        }
    }
}
