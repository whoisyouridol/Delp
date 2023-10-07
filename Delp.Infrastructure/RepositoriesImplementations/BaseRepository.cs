using Delp.Application.Repositories;

namespace Delp.Infrastructure.RepositoriesImplementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}