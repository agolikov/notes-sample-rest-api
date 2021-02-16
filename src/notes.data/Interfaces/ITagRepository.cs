using notes.data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace notes.data.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> InsertTagsAsync(string[] tags, Guid createdBy);
    }
}
