using System.Collections.ObjectModel;

namespace PrimeiraAPI.Models
{
    public class Categoria
    {

        // Uma boa pratica iniciar a collection na classe
        public Categoria()
        {
            Produtos=  new Collection<Produto>();   
        }
        public int CategoriaId { get; set; }
        public string? Nome { get; set; }
        public string? ImageUrl { get; set; }

        // ICollection é uma interface que representa um grupo de coisas, uma colecao generica com add, remove, count, clear.
        public ICollection<Produto>? Produtos { get; set; }
    }
}
