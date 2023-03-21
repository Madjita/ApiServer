using System.Linq.Expressions;

using Database.Context;
using Database.Contexts.Models;

namespace MobileDrill.Services.RepositoryFolder
{
    public interface IRepository<T> : IDisposable,IRepositorySync<T>, IRepositoryAsync<T> where T : BaseEntity
    {
        MyDbContext GetContext();
        Task LoadCollectionAsync(T entity, Expression<Func<T, IEnumerable<object>>> collectionExpression);
    }

    public interface IRepositorySync<T> where T : BaseEntity
    {
        SemaphoreSlim GetSemaphore();
        IQueryable<T> Get();
        List<T> GetAll();
        List<T> Where(Expression<Func<T, bool>> predicate);
    }

    public interface IRepositoryAsync<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<bool> AddAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> SaveAsync();
        Task<List<T>> toListAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,params Expression<Func<T, object>>[] includes);
        Task<List<object>> SelectAsync(Expression<Func<T, object>> predicate, CancellationToken cancellationToken = default);
        Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default);
        Task TruncateAsync(string tableName);
    }
}