namespace minimalapi_crud
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public bool Done { get; set; } = default!;
    }
}
