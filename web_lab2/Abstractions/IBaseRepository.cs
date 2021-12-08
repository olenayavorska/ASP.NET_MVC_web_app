using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace web_lab2.Abstractions
{
    public interface IBaseRepository<TEntity, in TKey> where TEntity : class, IEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(TKey id);
        
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TKey id);

        Task DeleteAsync(TEntity entity);

        Task<bool> ExistsAsync(TKey id);
    }
}