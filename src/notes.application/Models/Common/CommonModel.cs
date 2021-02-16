using System;

namespace notes.application.Models.Common
{
    public class CommonModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }
}
