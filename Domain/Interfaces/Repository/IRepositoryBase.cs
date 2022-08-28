using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<int> CountAsync();
        Task<int> DeleteAsync(T entity);
        int Delete(T entity);
        void Dispose();
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T?> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetAsync(int id);
        Task<int> SaveAsync();
        Task<T?> UpdateAsync(T? t, object? key);
    }
}
