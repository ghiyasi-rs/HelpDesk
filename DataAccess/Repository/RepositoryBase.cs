using DataAccess.Contexts;
using Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext appDbContext;

        protected RepositoryBase(AppDbContext context)
        {
            appDbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return appDbContext.Set<T>();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            var queryable = (IQueryable<T>)appDbContext.Set<T>();

            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await appDbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            appDbContext.Set<T>().Add(entity);
            await appDbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> match)
        {
            return await appDbContext.Set<T>().FirstOrDefaultAsync(match);
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await appDbContext.Set<T>().Where(match).ToListAsync();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            appDbContext.Set<T>().Remove(entity);
            return await appDbContext.SaveChangesAsync();
        }
        public virtual int Delete(T entity)
        {
            appDbContext.Set<T>().Remove(entity);
            return appDbContext.SaveChanges();
        }

        public virtual async Task<T?> UpdateAsync(T? t, object? key)
        {
            if (t == null)
                return null;

            var exist = await appDbContext.Set<T>().FindAsync(key);
            if (exist == null)
            {
                return exist;
            }

            appDbContext.Entry(exist).CurrentValues.SetValues(t);
            await appDbContext.SaveChangesAsync();

            return exist;
        }

        public async Task<int> CountAsync()
        {
            return await appDbContext.Set<T>().CountAsync();
        }

        public virtual async Task<int> SaveAsync()
        {
            return await appDbContext.SaveChangesAsync();
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                appDbContext.Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}