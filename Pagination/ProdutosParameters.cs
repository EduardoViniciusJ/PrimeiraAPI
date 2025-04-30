namespace PrimeiraAPI.Pagination
{
    public class ProdutosParameters
    {
        // Valor maximo permitido paras as páginas.
        const int maxPageSize = 50;

        // Número da página, por padrão 1.
        public int PageNumber { get; set; } = 1;

        // Tamanho da pagina (limite máximo definido) 
        private int _pageSize;


        // Propriedade que faz o controle do tamanho da pagina, limitado ao valor permitido
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize= (value > maxPageSize) ? maxPageSize : value;
            }
        }



    }
}
