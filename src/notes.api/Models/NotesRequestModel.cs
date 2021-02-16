using Microsoft.AspNetCore.Mvc;

namespace notes.api.Models
{
    public class NotesRequestModel : ListRequestModel
    {
        [FromQuery(Name = "title")]
        public string Title { get; set; }

        [FromQuery(Name = "text")]
        public string Text { get; set; }

        [FromQuery(Name = "tags")]
        public string[] Tags { get; set; }
    }
}
