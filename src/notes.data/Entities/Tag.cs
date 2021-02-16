using System.Collections.Generic;

namespace notes.data.Entities
{
    public class Tag : BaseEntity
    {
        public string Value { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
