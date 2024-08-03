namespace BlogAPI.DTOs.Response
{
    public class TagedPostResponse
    {
        public long? PostId { get; set; }

        public string? Title { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? AuthorName { get; set; }

        public List<SubCategoryResponse>? SubCategories { get; set; }
    }
}
