using notes.data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace notes.data.Interfaces
{
    public interface INoteRepository : IRepository<Note>
    {
        Task<IEnumerable<Note>> GetAsync(Guid? ownerId, int page, int size, bool ascSort, string sortBy,
            string title, string text, string[] tags);

        Task<long> CountAsync(Guid? ownerId, string title, string text, string[] tags);
    }
}
