using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Blog.Infrastructure
{
    public class DbContextRepository<TEntity, TKey> : DbContextRepository<TEntity>, IRepository<TEntity, TKey> where TEntity : class
    {
        public DbContextRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<TEntity> FindAsync(TKey id) => await DbContext.Set<TEntity>().FindAsync(id);
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
            var queryable = DbContext.Set<TEntity>();

            return predicate != null ? await queryable.AnyAsync(predicate) : await queryable.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var queryable = DbContext.Set<TEntity>();

            return predicate != null ? await queryable.CountAsync(predicate) : await queryable.CountAsync();
        }


        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, bool hasFilter = true)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();
            if (!hasFilter) query = query.IgnoreQueryFilters();
            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate) => await DbContext.Set<TEntity>().Where(predicate).Select(selector).SingleOrDefaultAsync();

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await FindAsync(predicate);

            return entity ?? throw new ArgumentNullException(typeof(TEntity).Name);
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate) => await DbContext.Set<TEntity>().Where(predicate).ToListAsync();

        public async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, bool hasFilter = true)
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

            if (!hasFilter)
            {
                query = query.IgnoreQueryFilters();
            }

            if (predicate != null) query = query.Where(predicate);
            return await query.Select(selector).ToListAsync();
        }

        public async Task<List<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, PagedRequest request, Expression<Func<TEntity, bool>> predicate = null) =>
            predicate != null
            ? await DbContext.Set<TEntity>().Where(predicate).Select(selector).OrderBy(request.Sort ?? "CreationTime Desc").Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync()
            : await DbContext.Set<TEntity>().Select(selector).OrderBy(request.Sort ?? "CreationTime Desc").Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
            await DbContext.SaveChangesAsync();
        }
    }

}
