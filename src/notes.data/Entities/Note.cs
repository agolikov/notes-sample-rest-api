using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace notes.data.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User Owner { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}