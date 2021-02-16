using System;

namespace notes.application.Models.Note
{
    public class UpdateNoteModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string[] Tags { get; set; }
    }
}