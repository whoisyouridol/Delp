namespace Delp.Application.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
    }
}