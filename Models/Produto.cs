namespace PrimeiraAPI.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        // propriedade de navegacao
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }



    }
}
