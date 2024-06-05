namespace BlogAPI.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int? AuthorId { get; set; }

        public int? CategoryId { get; set; }

    }
}
