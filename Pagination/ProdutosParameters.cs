namespace PrimeiraAPI.Pagination
{
    public class ProdutosParameters : QueryStringParameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; } // "menor", "maior", "igual"  



    }
}
