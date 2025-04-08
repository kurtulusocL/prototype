using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    public class EntityRepositoryBase<T, TContext> : IEntityRepository<T> where T : class, IEntity, new() where TContext : DbContext
    {
        private readonly TContext _context;

        public EntityRepositoryBase(TContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null for AddAsync method.");
                }
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the entity. Please check the database connection or constraints.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the entity.", ex);
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null for DeleteAsync method.");
                }

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the entity. Please check the database connection or constraints.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null for Get method.");
            }

            return _context.Set<T>().FirstOrDefault(filter);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null
            ? _context.Set<T>().ToList()
            : _context.Set<T>().Where(filter).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null
            ? await _context.Set<T>().ToListAsync()
            : await _context.Set<T>().Where(filter).ToListAsync();
        }

        public IEnumerable<T> GetAllByInclude(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return filter == null
                    ? query.ToList()
                    : query.Where(filter).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities with related data.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllByIncludeAsync(Expression<Func<T, bool>>[] filters, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        if (include != null)
                        {
                            query = query.Include(include);
                        }
                    }
                }

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        if (filter != null)
                        {
                            query = query.Where(filter);
                        }
                    }
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities with related data.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllIncludeByIdAsync(object id, string foreignKeyPropertyName, Expression<Func<T, bool>>[] conditions, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (conditions != null)
                {
                    foreach (var condition in conditions)
                    {
                        if (condition != null)
                        {
                            query = query.Where(condition);
                        }
                    }
                }

                if (id != null && !string.IsNullOrWhiteSpace(foreignKeyPropertyName))
                {
                    var parameter = Expression.Parameter(typeof(T), "entity");
                    var foreignKeyProperty = Expression.Property(parameter, foreignKeyPropertyName);
                    var propertyType = foreignKeyProperty.Type;

                    if (propertyType == typeof(int?) && id is int intId)
                    {
                        var idExpression = Expression.Convert(Expression.Constant(intId), typeof(int?));
                        query = query.Where(Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(foreignKeyProperty, idExpression),
                            parameter
                        ));
                    }
                    else if (propertyType == typeof(int) && id is int intIdNonNullable)
                    {
                        query = query.Where(Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(foreignKeyProperty, Expression.Constant(intIdNonNullable)),
                            parameter
                        ));
                    }
                    else if (propertyType == typeof(string) && id is string strId)
                    {
                        query = query.Where(Expression.Lambda<Func<T, bool>>(
                            Expression.Equal(foreignKeyProperty, Expression.Constant(strId)),
                            parameter
                        ));
                    }
                    else
                    {
                        throw new ArgumentException($"Unsupported ID type: {id.GetType()}");
                    }
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities with related data.", ex);
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null for GetAsync method.");
            }
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public T GetByInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null for GetAsync method.");
            }

            try
            {
                IQueryable<T> query = _context.Set<T>();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return query.FirstOrDefault(filter);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity with related data.", ex);
            }
        }

        public async Task<T> GetByIncludeAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null for GetAsync method.");
            }

            try
            {
                IQueryable<T> query = _context.Set<T>();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity with related data.", ex);
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity), "Entity cannot be null for UpdateAsync method.");
                }
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the entity. Please check the database connection or constraints.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}