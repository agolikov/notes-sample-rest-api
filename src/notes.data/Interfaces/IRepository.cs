using notes.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace notes.data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertOrUpdateAsync(T entity, Guid modifiedBy);
        Task<T> FindOneAsync(Expression<Func<T, bool>> filter, params string[] properties);
        Task<IEnumerable<T>> FindManyAsync(
            int page,
            int pageSize,
            bool ascSort,
            string sortByProperty,
            Expression<Func<T, bool>> filter,
            params string[] properties);
        Task<long> CountAsync(Expression<Func<T, bool>> filter = null, string[] properties = null);
        Task DeleteAsync(Guid id, Guid modifiedBy, bool hardDelete = false);
    }
}