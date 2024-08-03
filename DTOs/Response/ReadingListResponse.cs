namespace BlogAPI.DTOs.Response
{
    public class ReadingListResponse
    {
        public long ReadingListId { get; set; }
        public long PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
    }
}
