using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Database.Contexts.Models;
using Database.Context;
using Mars;


namespace MobileDrill.Services.RepositoryFolder
{
   public partial class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private MyDbContext _context;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public Repository(IServiceScopeFactory serviceScopeFactory,IDbContext context)
        {
            _context = (MyDbContext)context;

        }

        public SemaphoreSlim GetSemaphore()
        {
            return _semaphore;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public async Task<List<T>> toListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public MyDbContext GetContext()
        {
            return _context;
        }

        public async Task LoadCollectionAsync(T entity, Expression<Func<T, IEnumerable<object>>> collectionExpression)
        {
            await _semaphore.WaitAsync();
            EntityEntry<T> entityEntry = _context.Entry(entity);
            if(entityEntry.State is not EntityState.Modified)
            {
                entityEntry.State = EntityState.Modified;
            }
            await entityEntry.Collection(collectionExpression).LoadAsync();
            _semaphore.Release();
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            await _semaphore.WaitAsync();
            IEnumerable<T> result;
            try
            {
                result = await _context.Set<T>().ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
            return result;
        }

        private readonly SemaphoreSlim _semaphoreGetById = new SemaphoreSlim(1, 1);
        public async Task<T> GetByIdAsync(long id)
        {
            await _semaphoreGetById.WaitAsync();
            try
            {
                var result = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
                return result;
            }
            catch(NullReferenceException e)
            {
                Log.Error("Null reference exeption");
                throw new NullReferenceException($"Error: Repository<{typeof(T).Name}> \"GetByIdAsync\" {id} => {e.Message}");
            }
            finally
            {
              _semaphoreGetById.Release();
            }
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _semaphore.WaitAsync();
            try
            {
                 var result = await _context.Set<T>().AddAsync(entity);
            }
            catch(Exception e)
            {
                Logging.Log.Error(e.Message);
            }
            finally
            {
                _semaphore.Release();
            }
            
            return await SaveAsync();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            var result = _context.Set<T>().Remove(entity);
            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var state = _context.Entry<T>(entity).State;
            if(state is not EntityState.Modified)
            {
                Logging.Log.Information($"{typeof(T)}:{entity.Id} => {_context.Entry<T>(entity).State}","LocalDatabase");
                _context.Entry<T>(entity).State = EntityState.Modified;
            }
            var result = _context.Set<T>().Update(entity);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync();
            T result = null;
            try
            {
                result = await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
                if (result is not null)
                {
                    _context.Entry<T>(result).State = EntityState.Detached;
                }
            }
            catch(Exception e)
            {
                Log.ErrorAsync(e.Message).DisableAsyncWarning();
            }
            finally
            {
                _semaphore.Release();
            }

            return result;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,params Expression<Func<T, object>>[] includes)
        {
            await _semaphore.WaitAsync();
            T result = null;
            try
            {
                IQueryable<T> query = _context.Set<T>().Where(predicate);
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                result = await query.FirstOrDefaultAsync();
                if (result is not null)
                {
                    _context.Entry<T>(result).State = EntityState.Detached;
                }
            }
            catch(Exception e)
            {
                Log.ErrorAsync(e.Message).DisableAsyncWarning();
            }
            finally
            {
                _semaphore.Release();
            }
            return result;
        }

        public async Task<List<object>> SelectAsync(Expression<Func<T, object>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().Select(predicate).ToListAsync();
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsQueryable().Where(predicate).ToListAsync();
        }

        public async Task TruncateAsync(string tableName)
        {
           try
           {
                await _context.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName} WHERE ID < (SELECT MAX(`ID`) FROM {tableName})");
           }
           catch(Exception e)
           {
                Logging.Log.Error(e.Message);
           }
        }

        public List<T> Where(Expression<Func<T, bool>> predicate)
        {
           return _context.Set<T>().Where(predicate).ToList();
        }
    }
}