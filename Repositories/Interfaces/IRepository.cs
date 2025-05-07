using System.Linq.Expressions;

namespace PrimeiraAPI.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        // CRUD
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);

    }
}
