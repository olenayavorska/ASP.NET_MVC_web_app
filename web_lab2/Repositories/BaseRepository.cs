using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using web_lab2.Abstractions;
using web_lab2.Models;

namespace web_lab2.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly DatabaseContext Context;

        protected BaseRepository(DatabaseContext context)
        {
            Context = context;
        }

        protected virtual IQueryable<TEntity> ComplexEntities => Context.Set<TEntity>().AsNoTracking();

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await ComplexEntities.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await ComplexEntities.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await ComplexEntities.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await ComplexEntities.FirstOrDefaultAsync(predicate);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => Context.Set<TEntity>().Update(entity));
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await ComplexEntities.FirstAsync(en => en.Id.Equals(id));

            await Task.Run(() => Context.Set<TEntity>().Remove(entity));
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }
            
            await Task.Run(() => Context.Set<TEntity>().Remove(entity));
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            return await Context.Set<TEntity>().AnyAsync(it => it.Id.Equals(id));
        }
    }
}