using Microsoft.AspNetCore.Mvc;

namespace notes.api.Models
{
    public class ListRequestModel
    {
        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        [FromQuery(Name = "size")]
        public int? Size { get; set; }

        [FromQuery(Name = "is_asc_sort")]
        public string IsAscSort { get; set; }

        [FromQuery(Name = "sort_by")]
        public string SortProperty { get; set; }
    }
}