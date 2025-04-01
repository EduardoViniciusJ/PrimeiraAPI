using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Context;
using PrimeiraAPI.Models;

namespace PrimeiraAPI.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;


         }

        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return categoria;  
            
        }

        public Categoria Detele(int id)

        {
            var categoria = _context.Categorias.Find(id);
            if (categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;

        }
    }
}
