namespace CommentApi.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string PostId { get; set; }
        public string AuthorName { get; set; }
        public string? AuthorEmail { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
