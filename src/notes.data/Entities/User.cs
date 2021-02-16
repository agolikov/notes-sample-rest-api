using System.Collections.Generic;

namespace notes.data.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}