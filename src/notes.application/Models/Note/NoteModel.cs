using notes.application.Models.Common;

namespace notes.application.Models.Note
{
    public class NoteModel : CommonModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string[] Tags { get; set; }
    }
}