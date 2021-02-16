using System.Collections.Generic;

namespace notes.application.Models.Common
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
