namespace PrimeiraAPI.Pagination
{
    public class PageList<T> : List<T> where T : class
    {
        public int CurrentPage { get; set; } // Representa a página atual   
        public int TotalPages { get; set; } // Total de páginas
        public int PageSize { get; set; } // Armazena o numero de itens  por página
        public int TotalCount { get; set; }  // Numero total de elemento na fonte de dados


        public bool HasPrevious => CurrentPage > 1;  // Verifica se há uma página anterior
        public bool HasNext => CurrentPage < TotalPages;  // Verifica se há uma próxima página


        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); //

            AddRange(items);
        }

        public static PageList<T> ToPagedList(IQueryable<T> surce, int pageNumber, int pageSize)
        {
            var count = surce.Count(); // Conta o total de elementos
            var items = surce.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); // Pega os elementos da página atual

            return new PageList<T>(items, count, pageNumber, pageSize); // Retorna a lista paginada
        }









    }
}
