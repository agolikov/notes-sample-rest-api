using notes.data.Entities;
using notes.data.Extensions;
using notes.data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace notes.data.Repositories
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext context) : base(context)
        {
        }

        private Expression<Func<Note, bool>> GetFilterExpression(Guid? ownerId, string title, string text,
            string[] tags)
        {
            Expression<Func<Note, bool>> exp = null;
            if (ownerId != null)
            {
                exp = e => e.CreatedBy == ownerId;
            }

            if (title != null)
            {
                exp = QueryableExtensions.AndAlso(exp, e => e.Title.Contains(title));
            }

            if (text != null)
            {
                exp = QueryableExtensions.AndAlso(exp, e => e.Text.Contains(text));
            }

            if (tags != null && tags.Any())
            {
                Expression<Func<Note, bool>> expTags = null;
                foreach (var tag in tags)
                {
                    expTags = expTags == null
                        ? e => e.Tags.Any(t => t.Value == tag)
                        : QueryableExtensions.CombineOr(expTags, e => e.Tags.Any(t => t.Value == tag));
                }

                exp = QueryableExtensions.AndAlso(exp, expTags);
            }

            return exp;
        }

        public async Task<IEnumerable<Note>> GetAsync(Guid? ownerId, int page, int size, bool ascSort, string sortBy,
            string title, string text,
            string[] tags)
        {
            var filterExpression = GetFilterExpression(ownerId, title, text, tags);
            return await FindManyAsync(page, size, ascSort, sortBy, filterExpression, "Tags");
        }

        public async Task<long> CountAsync(Guid? ownerId, string title, string text, string[] tags)
        {
            var filterExpression = GetFilterExpression(ownerId, title, text, tags);
            return await CountAsync(filterExpression, "Tags");
        }
    }
}