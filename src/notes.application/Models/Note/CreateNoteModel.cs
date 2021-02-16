namespace notes.application.Models.Note
{
    public class CreateNoteModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string[] Tags { get; set; }
    }
}