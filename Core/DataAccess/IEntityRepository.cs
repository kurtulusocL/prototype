using System.Linq.Expressions;
using Core.Entities;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetAllByIncludeAsync(Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<T>> GetAllIncludeByIdAsync(object id, string foreignKeyPropertyName, Expression<Func<T, bool>>[] conditions, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIncludeAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> GetAllByInclude(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);       
        T Get(Expression<Func<T, bool>> filter);
        T GetByInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}