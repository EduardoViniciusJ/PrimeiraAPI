using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeiraAPI.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        // Uma boa pratica iniciar a collection na classe
        public Categoria()
        {
            Produtos=  new Collection<Produto>();   
        }
        [Key]
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImageUrl { get; set; }

        // ICollection é uma interface que representa um grupo de coisas, uma colecao generica com add, remove, count, clear.
        public ICollection<Produto>? Produtos { get; set; }
    }
}
