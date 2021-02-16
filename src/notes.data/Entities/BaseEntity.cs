using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace notes.data.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }

        public BaseEntity()
        {
            Created = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }
    }
}
