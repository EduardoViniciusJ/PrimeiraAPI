using PrimeiraAPI.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrimeiraAPI.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no mínimo {2} caracteres",
        MinimumLength = 5)]
        [PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }



        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }



        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        // propriedade de navegacao

        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }



    }
}
