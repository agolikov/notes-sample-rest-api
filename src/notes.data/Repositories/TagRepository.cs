using Microsoft.EntityFrameworkCore;
using notes.data.Entities;
using notes.data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace notes.data.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> InsertTagsAsync(string[] tags, Guid createdBy)
        {
            List<Tag> result = new List<Tag>();
            var existedTags = await Context.Set<Tag>().Where(t => tags.Contains(t.Value)).ToListAsync();
            result.AddRange(existedTags);

            bool addedAny = false;
            foreach (var tagString in tags)
            {
                if (existedTags.All(t => t.Value != tagString))
                {
                    var tag = new Tag { Value = tagString, CreatedBy = createdBy };
                    await Context.Set<Tag>().AddAsync(tag);
                    result.Add(tag);
                    addedAny = true;
                }
            }

            if (addedAny)
            {
                await Context.SaveChangesAsync();
            }

            return result;
        }
    }
}