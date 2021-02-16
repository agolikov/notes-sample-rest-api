using Microsoft.EntityFrameworkCore;
using notes.data.Entities;
using notes.data.Exceptions;
using notes.data.Extensions;
using notes.data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace notes.data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DbContext Context;

        protected Repository(DbContext context)
        {
            Context = context;
        }

        public async Task<T> InsertOrUpdateAsync(T entity, Guid modifiedBy)
        {
            var existedEntity = await FindOneAsync(t => t.Id == entity.Id);
            if (existedEntity == null)
            {
                entity.Created = DateTime.UtcNow;
                entity.CreatedBy = modifiedBy;
                entity.Version = 1;
                await Context.Set<T>().AddAsync(entity);
            }
            else
            {
                if (existedEntity.Version != entity.Version)
                {
                    throw existedEntity.Id.VersionInNotCorrectException();
                }

                entity.Modified = DateTimeProvider.GetCurrentDateTime();
                entity.ModifiedBy = modifiedBy;
                entity.CreatedBy = existedEntity.CreatedBy;
                entity.Created = existedEntity.Created;
                entity.IsDeleted = false;
                entity.Version += 1;
                Context.Entry(existedEntity).State = EntityState.Detached;
                Context.Entry(entity).State = EntityState.Modified;
                Context.Set<T>().Update(entity);
            }
            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filter, params string[] properties)
        {
            IQueryable<T> entity = Context.Set<T>();
            if (properties != null)
            {
                entity = Context.Set<T>();
                foreach (var property in properties)
                {
                    entity = entity.Include(property);
                }
            }

            var e = await entity.FirstOrDefaultAsync(filter);
            if (e != null && e.IsDeleted)
            {
                return null;
            }
            return e;
        }

        public async Task DeleteAsync(Guid id, Guid modifiedBy, bool hardDelete = false)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                throw new DalException(ErrorCodes.EntityNotFound, id);
            }

            if (hardDelete)
            {
                Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync();
            }
            else
            {
                entity.IsDeleted = true;
                entity.Modified = DateTimeProvider.GetCurrentDateTime();
                entity.ModifiedBy = modifiedBy;
                Context.Set<T>().Update(entity);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindManyAsync(int page, int pageSize, bool ascSort, string sortByProperty, Expression<Func<T, bool>> filter,
            params string[] properties)
        {
            IQueryable<T> entities = Context.Set<T>().Where(t => !t.IsDeleted);
            if (properties != null)
            {
                entities = Context.Set<T>();
                foreach (var property in properties)
                {
                    entities = entities.Include(property);
                }
            }

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            if (sortByProperty == null)
            {
                sortByProperty = "Id";
            }

            entities = entities.OrderBy(sortByProperty, ascSort ? ListSortDirection.Ascending : ListSortDirection.Descending);

            entities = entities.Skip((page - 1) * pageSize).Take(pageSize);

            return await entities.ToListAsync();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> filterExpression = null, params string[] properties)
        {
            IQueryable<T> entities = Context.Set<T>().Where(t => !t.IsDeleted);
            if (properties != null)
            {
                entities = Context.Set<T>();
                foreach (var property in properties)
                {
                    entities = entities.Include(property);
                }
            }

            if (filterExpression != null)
            {
                return await entities.LongCountAsync(filterExpression);
            }

            return await entities.LongCountAsync();
        }
    }
}