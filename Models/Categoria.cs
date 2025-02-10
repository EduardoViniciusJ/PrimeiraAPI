using System.Collections.ObjectModel;

namespace PrimeiraAPI.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Produtos=  new Collection<Produto>();   
        }


        public int CategoriaId { get; set; }
        public string? Nome { get; set; }
        public string? ImageUrl { get; set; }

        // ICollection é uma interface que representa um grupo de coisas, uma colecao generica.
        public ICollection<Produto>? Produtos { get; set; }


    }
}
