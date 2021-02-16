namespace notes.application.Models.Common
{
    public class ListQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public bool IsAscSort { get; set; }

        public string SortProperty { get; set; }
    }
}