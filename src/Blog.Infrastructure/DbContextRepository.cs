using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
    public class DbContextRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly BlogDbContext DbContext;

        public DbContextRepository(BlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ? await DbContext.Set<TEntity>().AnyAsync(predicate) : await DbContext.Set<TEntity>().AnyAsync();
        }

        public async Task<TEntity> GetAsync(TKey key)
        {
            return await DbContext.Set<TEntity>().FindAsync(key);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false)
        {
            return asNoTracking
                       ? await DbContext.Set<TEntity>().Where(predicate).AsNoTracking().Select(selector)
                                        .SingleOrDefaultAsync()
                       : await DbContext.Set<TEntity>().Where(predicate).Select(selector)
                                        .SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(bool asNoTracking = false)
        {
            if (asNoTracking)
                return await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {
            return asNoTracking
                       ? await DbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync()
                       : await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public Task<IQueryable<TEntity>> GetQueryableAsync(bool asNoTracking = false)
        {
            return Task.FromResult(asNoTracking ? DbContext.Set<TEntity>().AsNoTracking().AsQueryable() : DbContext.Set<TEntity>().AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {
            return Task.FromResult(asNoTracking ? DbContext.Set<TEntity>().Where(predicate).AsNoTracking().AsQueryable() : DbContext.Set<TEntity>().Where(predicate).AsQueryable());
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                              bool asNoTracking = false)
        {
            if (asNoTracking)
                return await DbContext.Set<TEntity>().AsNoTracking().Select(selector).ToListAsync();
            return await DbContext.Set<TEntity>().Select(selector).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey key)
        {
            var entity = await GetAsync(key);

            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetListAsync(predicate);

            if (entities.Any())
            {
                DbContext.Set<TEntity>().RemoveRange(entities);
                await DbContext.SaveChangesAsync();
            }
        }
    }

    public class DbContextRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BlogDbContext DbContext;

        public DbContextRepository(BlogDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null ? await DbContext.Set<TEntity>().AnyAsync(predicate) : await DbContext.Set<TEntity>().AnyAsync();
        }

        public async Task<TEntity> GetAsync(object key)
        {
            return await DbContext.Set<TEntity>().FindAsync(key);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, bool asNoTracking = false)
        {
            return asNoTracking
                       ? await DbContext.Set<TEntity>().Where(predicate).AsNoTracking().Select(selector)
                                        .SingleOrDefaultAsync()
                       : await DbContext.Set<TEntity>().Where(predicate).Select(selector)
                                        .SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetListAsync(bool asNoTracking = false)
        {
            if (asNoTracking)
                return await DbContext.Set<TEntity>().AsNoTracking().ToListAsync();
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {
            if (asNoTracking)
                return await DbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
            return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public Task<IQueryable<TEntity>> GetQueryableAsync(bool asNoTracking = false)
        {
            return Task.FromResult(asNoTracking ? DbContext.Set<TEntity>().AsNoTracking().AsQueryable() : DbContext.Set<TEntity>().AsQueryable());
        }

        public Task<IQueryable<TEntity>> GetQueryableAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {
            return Task.FromResult(asNoTracking ? DbContext.Set<TEntity>().Where(predicate).AsNoTracking().AsQueryable() : DbContext.Set<TEntity>().Where(predicate).AsQueryable());
        }

        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                              bool asNoTracking = false)
        {
            if (asNoTracking)
                return await DbContext.Set<TEntity>().AsNoTracking().Select(selector).ToListAsync();
            return await DbContext.Set<TEntity>().Select(selector).ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(object key)
        {
            var entity = await GetAsync(key);

            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await GetListAsync(predicate);

            if (entities.Any())
            {
                DbContext.Set<TEntity>().RemoveRange(entities);
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
